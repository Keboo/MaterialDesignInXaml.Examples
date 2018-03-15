using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.DwayneNeed.Media.Imaging
{
    /// <summary>
    ///     The ChainedBitmap class is the base class for custom bitmaps that
    ///     processes the content of another source.
    /// </summary>
    /// <remarks>
    ///     The default implementation of the BitmapSource virtuals is to
    ///     delegate to the source.  This makes sense for most properties like
    ///     DpiX, DpiY, PixelWidth, PixelHeight, etc, as in many scenarios
    ///     these properties are the same for the entire chain of bitmap
    ///     sources.  However, derived classes should pay special attention to
    ///     the Format property.  Many bitmap processors only support a limited
    ///     number of pixel formats, and they should return this for the
    ///     Format property.  ChainedBitmap will take care of converting the
    ///     pixel format as needed in the base implementation of CopyPixels.
    /// </remarks>
    public class ChainedBitmap : CustomBitmap
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable disposable = Source as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                Source = null;
            }

            base.Dispose(disposing);
        }

        #region Freezable

        /// <summary>
        ///     Creates an instance of the ChainedBitmap class.
        /// </summary>
        /// <returns>
        ///     The new instance.  If you derive from this class, you must
        ///     override this method to return your own type.
        /// </returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ChainedBitmap();
        }

        /// <summary>
        ///     Transitions this instance into a thread-safe, read-only mode.
        /// </summary>
        /// <param name="isChecking">
        ///     Whether or not the transition should really happen, or just to
        ///     determine if the transition is possible.
        /// </param>
        /// <remarks>
        ///     Override this method if you have additional non-DP state that
        ///     should be frozen along with the instance.
        /// </remarks>
        protected override bool FreezeCore(bool isChecking)
        {
            if (_formatConverter != null)
            {
                if (isChecking)
                {
                    return _formatConverter.CanFreeze;
                }
                else
                {
                    _formatConverter.Freeze();
                }
            }

            return true;
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="original">
        ///     The original instance to copy data from.
        /// </param>
        /// <param name="useCurrentValue">
        ///     Whether or not to copy the current value of expressions, or the
        ///     expressions themselves.
        /// </param>
        /// <param name="willBeFrozen">
        ///     Indicates whether or not the clone will be frozen.  If the
        ///     clone will be immediately frozen, there is no need to clone
        ///     data that is already frozen, you can just share the instance.
        /// </param>
        /// <remarks>
        ///     Override this method if you have additional non-DP state that
        ///     should be transfered to clones.
        /// </remarks>
        protected override void CopyCore(CustomBitmap original, bool useCurrentValue, bool willBeFrozen)
        {
            ChainedBitmap originalChainedBitmap = (ChainedBitmap)original;
            if (originalChainedBitmap._formatConverter != null)
            {
                if (useCurrentValue)
                {
                    if (willBeFrozen)
                    {
                        _formatConverter = (FormatConvertedBitmap)originalChainedBitmap._formatConverter.GetCurrentValueAsFrozen();
                    }
                    else
                    {
                        _formatConverter = (FormatConvertedBitmap)originalChainedBitmap._formatConverter.CloneCurrentValue();
                    }
                }
                else
                {
                    if (willBeFrozen)
                    {
                        _formatConverter = (FormatConvertedBitmap)originalChainedBitmap._formatConverter.GetAsFrozen();
                    }
                    else
                    {
                        _formatConverter = (FormatConvertedBitmap)originalChainedBitmap._formatConverter.Clone();
                    }
                }
            }
        }

        #endregion Freezable

        #region Source

        /// <summary>
        ///     The DependencyProperty for the Source property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
              DependencyProperty.Register("Source",
                               typeof(BitmapSource),
                               typeof(ChainedBitmap),
                               new FrameworkPropertyMetadata(OnSourcePropertyChanged));

        /// <summary>
        ///     The BitmapSource to chain.
        /// </summary>
        public BitmapSource Source
        {
            get
            {
                return (BitmapSource)GetValue(SourceProperty);
            }

            set
            {
                SetValue(SourceProperty, value);
            }
        }

        protected virtual void OnSourcePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            // Stop listening for the download and decode events on the old source.
            BitmapSource oldValue = e.OldValue as BitmapSource;
            if (oldValue != null && !oldValue.IsFrozen)
            {
                oldValue.DownloadCompleted -= OnSourceDownloadCompleted;
                oldValue.DownloadProgress -= OnSourceDownloadProgress;
                oldValue.DownloadFailed -= OnSourceDownloadFailed;
                oldValue.DecodeFailed -= OnSourceDecodeFailed;
            }

            // Start listening for the download and decode events on the new source.
            BitmapSource newValue = e.NewValue as BitmapSource;
            if (newValue != null && !newValue.IsFrozen)
            {
                newValue.DownloadCompleted += OnSourceDownloadCompleted;
                newValue.DownloadProgress += OnSourceDownloadProgress;
                newValue.DownloadFailed += OnSourceDownloadFailed;
                newValue.DecodeFailed += OnSourceDecodeFailed;
            }
        }

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChainedBitmap chainedBitmap = (ChainedBitmap)d;
            chainedBitmap.OnSourcePropertyChanged(e);
        }

        #endregion Source

        #region BitmapSource Properties

        /// <summary>
        ///     Horizontal DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override double DpiX
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.DpiX;
                }
                else
                {
                    return base.DpiX;
                }
            }
        }

        /// <summary>
        ///     Vertical DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override double DpiY
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.DpiY;
                }
                else
                {
                    return base.DpiY;
                }
            }
        }

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override PixelFormat Format
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.Format;
                }
                else
                {
                    return base.Format;
                }
            }
        }

        /// <summary>
        ///     Width of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override int PixelWidth
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.PixelWidth;
                }
                else
                {
                    return base.PixelWidth;
                }
            }
        }

        /// <summary>
        ///     Height of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override int PixelHeight
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.PixelHeight;
                }
                else
                {
                    return base.PixelHeight;
                }
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override BitmapPalette Palette
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.Palette;
                }
                else
                {
                    return base.Palette;
                }
            }
        }

        #endregion BitmapSource Properties

        #region BitmapSource Download

        /// <summary>
        ///     Whether or not the BitmapSource is downloading content.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override bool IsDownloading
        {
            get
            {
                BitmapSource source = Source;
                if (source != null)
                {
                    return source.IsDownloading;
                }
                else
                {
                    return false;
                }
            }
        }

        private void OnSourceDownloadCompleted(object sender, EventArgs e)
        {
            RaiseDownloadCompleted();
        }

        private void OnSourceDownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            RaiseDownloadProgress(e);
        }

        private void OnSourceDownloadFailed(object sender, ExceptionEventArgs e)
        {
            RaiseDownloadFailed(e);
        }

        #endregion BitmapSource Download

        #region BitmapSource Decode

        private void OnSourceDecodeFailed(object sender, ExceptionEventArgs e)
        {
            RaiseDecodeFailed(e);
        }

        #endregion BitmapSource Decode

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
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        protected override void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
        {
            BitmapSource source = Source;
            BitmapSource convertedSource = source;

            if (source != null)
            {
                PixelFormat sourceFormat = source.Format;
                PixelFormat destinationFormat = Format;

                if (sourceFormat != destinationFormat)
                {
                    // We need a format converter.  Reuse the cached one if
                    // it still matches.
                    if (_formatConverter == null ||
                        _formatConverter.Source != source ||
                        _formatConverter.Format != destinationFormat ||
                        _formatConverterSourceFormat != sourceFormat)
                    {
                        WritePreamble();
                        _formatConverterSourceFormat = sourceFormat;
                        _formatConverter = new FormatConvertedBitmap(source, destinationFormat, Palette, 0);
                        WritePostscript();
                    }

                    convertedSource = _formatConverter;
                }

                convertedSource.CopyPixels(sourceRect, buffer, bufferSize, stride);
            }
        }

        private PixelFormat _formatConverterSourceFormat;
        private FormatConvertedBitmap _formatConverter;

        #endregion BitmapSource CopyPixels
    }
}
