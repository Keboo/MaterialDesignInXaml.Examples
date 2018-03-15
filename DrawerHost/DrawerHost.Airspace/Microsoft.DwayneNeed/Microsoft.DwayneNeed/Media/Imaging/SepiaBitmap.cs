using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.DwayneNeed.Media.Imaging
{
    /// <summary>
    ///     The SepiaBitmap class processes a source and converts the
    ///     image to a sepia color scheme.
    /// </summary>
    /// <remarks>
    ///     For simplicity, we only process Bgr32 or Bgra32 formatted bitmaps,
    ///     anything else is converted to one of those formats. Bgr32 and
    ///     Bgra32 share the same memory layout for the RGB channels.
    /// </remarks>
    public class SepiaBitmap : ChainedBitmap
    {
        #region BitmapSource Properties

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     We will work natively with either Bgr32 or Bgra32, since both
        ///     formats have the same memory layout.
        /// </remarks>
        public sealed override PixelFormat Format
        {
            get
            {
                // Preserve an alpha channel, if there is one.
                PixelFormat format = base.Format;
                if (format == PixelFormats.Bgra32 ||
                    format == PixelFormats.Prgba64 ||
                    format == PixelFormats.Pbgra32 ||
                    format == PixelFormats.Prgba128Float ||
                    format == PixelFormats.Rgba128Float ||
                    format == PixelFormats.Rgba64)
                {
                    format = PixelFormats.Bgra32;
                }
                else
                {
                    format = PixelFormats.Bgr32;
                }

                return format;
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     We only support Bgr32 and Bgra32 pixel formats, so a palette
        ///     is never needed, so we return null.
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
        ///     Converts the contents of the source bitmap into a sepia color
        ///     scheme.
        ///     
        ///     The algorithm is taken from here:
        ///     http://msdn.microsoft.com/en-us/magazine/cc163866.aspx
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

                // The buffer has been filled with Bgr32 or Bgra32 pixels.
                // Now process those pixels into a sepia tint.  Ignore the
                // alpha channel.
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
                            // Get the linear color space values of this pixel.
                            Color c = Color.FromRgb(pPixel->Red, pPixel->Green, pPixel->Blue);
                            float red = c.ScR;
                            float green = c.ScG;
                            float blue = c.ScB;

                            Color cSepia = Color.FromScRgb(1.0f,
                                                           red * 0.393f + green * 0.769f + blue * 0.189f,
                                                           red * 0.349f + green * 0.686f + blue * 0.168f,
                                                           red * 0.272f + green * 0.534f + blue * 0.131f);


                            // Write sRGB (non-linear) since it is implied by
                            // the pixel format we chose.
                            pPixel->Red = cSepia.R;
                            pPixel->Green = cSepia.G;
                            pPixel->Blue = cSepia.B;

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
