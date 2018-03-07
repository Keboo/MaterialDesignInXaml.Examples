using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.DwayneNeed.Media.Imaging
{
    /// <summary>
    ///     The ColorKeyBitmap class processes a source and produces
    ///     Bgra32 bits, where all source pixels are opaque except for one
    ///     color that is transparent.
    /// </summary>
    /// <remarks>
    ///     For simplicity, we only process Bgra32 formatted bitmaps, anything
    ///     else is converted to that format.  Converting a pixel format that
    ///     does not have an alpha channel to Bgra32 simply creates an opaque
    ///     alpha value.  ColorKeyBitmap then changes the alpha channel for
    ///     pixels that match the transparent color.
    /// </remarks>
    public class ColorKeyBitmap : ChainedBitmap
    {
        #region TransparentColor

        /// <summary>
        ///     The DependencyProperty for the TransparentColor property.
        /// </summary>
        public static readonly DependencyProperty TransparentColorProperty =
              DependencyProperty.Register("TransparentColor",
                               typeof(Color?),
                               typeof(ColorKeyBitmap),
                               new FrameworkPropertyMetadata(
                                   null,
                                   null,
                                   null));

        /// <summary>
        ///     The color to make transparent.
        /// </summary>
        public Color? TransparentColor
        {
            get
            {
                return (Color?)GetValue(TransparentColorProperty);
            }

            set
            {
                SetValue(TransparentColorProperty, value);
            }
        }

        #endregion TransparentColor

        #region BitmapSource Properties

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     ColorKeyBitmap will only work natively with Bgra32.
        /// </remarks>
        public sealed override PixelFormat Format
        {
            get
            {
                return PixelFormats.Bgra32;
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     We only support Bgra32, so a palette is never needed, so we
        ///     return null.
        /// </remarks>
        public override BitmapPalette Palette
        {
            get
            {
                return null;
            }
        }

        #endregion BitmapSource Properties

        #region BitmapSource CopyPixels

        /// <summary>
        ///     Requests pixels from the ChainedCustomBitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels being requested. 
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination buffer.
        /// </param>
        /// <param name="bufferSize">
        ///     The size of the destination buffer.
        /// </param>
        /// <param name="buffer">
        ///     The destination buffer.
        /// </param>
        /// <remarks>
        ///     The default implementation simply calls CopyPixels on the
        ///     source.
        /// </remarks>
        protected override void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
        {
            BitmapSource source = Source;
            if (source != null)
            {
                // First defer to the base implementation, which will fill in
                // the buffer from the source and convert the pixel format as
                // needed.
                base.CopyPixelsCore(sourceRect, stride, bufferSize, buffer);

                // Also fetch the color of the upper-left corner (0,0) if the
                // transparent color has not been specified.
                Color transparentColor;
                if (TransparentColor == null)
                {
                    uint[] firstPixel = new uint[1];

                    unsafe
                    {
                        fixed (uint* pFirstPixel = firstPixel)
                        {
                            base.CopyPixelsCore(new Int32Rect(0, 0, 1, 1), 4, 4, new IntPtr(pFirstPixel));

                            Bgra32Pixel* pBgraPixel = (Bgra32Pixel*)pFirstPixel;
                            transparentColor = Color.FromRgb(pBgraPixel->Red,
                                                             pBgraPixel->Green,
                                                             pBgraPixel->Blue);
                        }
                    }
                }
                else
                {
                    transparentColor = TransparentColor.Value;
                }

                // The buffer has been filled with Bgr32 or Bgra32 pixels.
                // Now process these pixels and set the alpha channel to 0 for
                // pixels that match the color key.  Leave the other pixels
                // alone.
                //
                // Note: if this buffer pointer came from a managed array, the
                // array has already been pinned.
                unsafe
                {
                    byte* pBytes = (byte*)buffer.ToPointer();
                    for (int y = 0; y < sourceRect.Height; y++)
                    {
                        Bgra32Pixel* pPixel = (Bgra32Pixel*)pBytes;

                        for (int x = 0; x < sourceRect.Width; x++)
                        {
                            if (pPixel->Red == transparentColor.R &&
                                pPixel->Green == transparentColor.G &&
                                pPixel->Blue == transparentColor.B)
                            {
                                pPixel->Alpha = 0x00;
                            }

                            pPixel++;
                        }

                        pBytes += stride;
                    }
                }
            }
        }

        #endregion BitmapSource CopyPixels
    }
}
