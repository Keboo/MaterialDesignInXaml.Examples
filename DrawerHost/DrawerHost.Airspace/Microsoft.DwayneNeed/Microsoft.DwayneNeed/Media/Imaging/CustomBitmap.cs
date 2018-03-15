using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Microsoft.DwayneNeed.Media.Imaging
{
    public class CustomBitmap : BitmapSource, IDisposable
    {
        #region Freezable

        /// <summary>
        ///     Creates an instance of a CustomBitmap.
        /// </summary>
        /// <returns>
        ///     The new instance.
        /// </returns>
        /// <remarks>
        ///     The default implementation uses reflection to create a new
        ///     instance of this type.  Derived classes may override this
        ///     method if they wish to provide a more performant
        ///     implementation, or if their type does not have a public
        ///     default constructor.
        /// </remarks>
        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(this.GetType());
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     When Freezable is cloned, WPF will make deep clones of all
        ///     writable, locally-set properties including expressions. The
        ///     property's base value is copied -- not the current value. WPF
        ///     skips read only DPs.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transfered to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void CloneCore(Freezable source)
        {
            base.CloneCore(source);

            CustomBitmap customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ false, /*willBeFrozen*/ false);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     When a Freezable's "current value" is cloned, WPF will make
        ///     deep clones of the "current values" of all writable,
        ///     locally-set properties. This has the effect of resolving
        ///     expressions to their values. WPF skips read only DPs.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transfered to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void CloneCurrentValueCore(Freezable source)
        {
            base.CloneCurrentValueCore(source);

            CustomBitmap customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ true, /*willBeFrozen*/ false);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     Freezable.GetAsFrozen is semantically equivalent to
        ///     Freezable.Clone().Freeze(), except that you can avoid copying
        ///     any portions of the Freezable graph which are already frozen.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transfered to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void GetAsFrozenCore(Freezable source)
        {
            base.GetAsFrozenCore(source);

            CustomBitmap customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ false, /*willBeFrozen*/ true);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     Freezable.GetCurrentValueAsFrozen is semantically equivalent to
        ///     Freezable.CloneCurrentValue().Freeze(), except that WPF will
        ///     avoid copying any portions of the Freezable graph which are
        ///     already frozen.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transfered to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            base.GetCurrentValueAsFrozenCore(source);

            CustomBitmap customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ true, /*willBeFrozen*/ true);
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
        protected virtual void CopyCore(CustomBitmap original, bool useCurrentValue, bool willBeFrozen)
        {
        }

        #endregion Freezable

        #region BitmapSource Properties

        /// <summary>
        ///     Horizontal DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override double DpiX
        {
            get
            {
                return 96.0;
            }
        }

        /// <summary>
        ///     Vertical DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override double DpiY
        {
            get
            {
                return (double)96.0;
            }
        }

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override PixelFormat Format
        {
            get
            {
                return PixelFormats.Bgr32;
            }
        }

        /// <summary>
        ///     Width of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override int PixelWidth
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        ///     Height of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override int PixelHeight
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
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
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="pixels">
        ///     The destination array of pixels.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination array.
        /// </param>
        /// <param name="offset">
        ///     The starting index within the destination array to copy to.
        /// </param>
        /// <remarks>
        ///     Derived classes must override CopyPixelsCommon to implement
        ///     custom logic.
        /// </remarks>
        public sealed override void CopyPixels(Array pixels, int stride, int offset)
        {
            Int32Rect sourceRect = new Int32Rect(0, 0, PixelWidth, PixelHeight);
            CopyPixels(sourceRect, pixels, stride, offset);
        }

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels to copy.
        /// </param>
        /// <param name="pixels">
        ///     The destination array of pixels.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination array.
        /// </param>
        /// <param name="offset">
        ///     The starting index within the destination array to copy to.
        /// </param>
        /// <remarks>
        ///     Derived classes must override CopyPixelsCommon to implement
        ///     custom logic.
        /// </remarks>
        public sealed override void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            int elementSize;
            int bufferSize;
            Type elementType;
            ValidateArrayAndGetInfo(pixels, out elementSize, out bufferSize, out elementType);

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            // We accept arrays of arbitrary value types - but not reference types.
            if (elementType == null || !elementType.IsValueType)
            {
                throw new ArgumentException("pixels");
            }

            checked
            {
                int offsetInBytes = offset * elementSize;
                if (offsetInBytes >= bufferSize)
                {
                    throw new IndexOutOfRangeException();
                }

                // Get the address of the data in the array by pinning it.
                GCHandle arrayHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                try
                {
                    // Adjust the buffer and bufferSize to account for the offset.
                    IntPtr buffer = arrayHandle.AddrOfPinnedObject();
                    buffer = new IntPtr(((long)buffer) + (long)offsetInBytes);
                    bufferSize -= offsetInBytes;

                    CopyPixels(sourceRect, buffer, bufferSize, stride);
                }
                finally
                {
                    arrayHandle.Free();
                }
            }
        }

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels to copy.
        /// </param>
        /// <param name="buffer">
        ///     The destination buffer of pixels.
        /// </param>
        /// <param name="bufferSize">
        ///     The size of the buffer, in bytes.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination buffer.
        /// </param>
        /// <remarks>
        ///     Derived classes must override CopyPixelsCommon to implement
        ///     custom logic.
        /// </remarks>
        public sealed override void CopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride)
        {
            // WIC would specify NULL for the source rect to indicate that the
            // entire content should be copied.  WPF turns that into an empty
            // rect, which we inflate here to be the entire bounds.
            if (sourceRect.IsEmpty)
            {
                sourceRect.Width = PixelWidth;
                sourceRect.Height = PixelHeight;
            }

            if (sourceRect.X < 0)
            {
                throw new ArgumentOutOfRangeException("sourceRect");
            }

            if (sourceRect.Width < 0)
            {
                throw new ArgumentOutOfRangeException("sourceRect");
            }

            if ((sourceRect.X + sourceRect.Width) > PixelWidth)
            {
                throw new ArgumentOutOfRangeException("sourceRect");
            }

            if (sourceRect.Y < 0)
            {
                throw new ArgumentOutOfRangeException("sourceRect");
            }

            if (sourceRect.Height < 0)
            {
                throw new ArgumentOutOfRangeException("sourceRect");
            }

            if ((sourceRect.Y + sourceRect.Height) > PixelHeight)
            {
                throw new ArgumentOutOfRangeException("sourceRect");
            }

            if (buffer == IntPtr.Zero)
            {
                throw new ArgumentNullException("buffer");
            }

            checked
            {
                if (stride < 1)
                {
                    throw new ArgumentOutOfRangeException("stride");
                }

                uint minStrideInBits = (uint)(sourceRect.Width * Format.BitsPerPixel);
                uint minStrideInBytes = ((minStrideInBits + 7) / 8);
                if (stride < minStrideInBytes)
                {
                    throw new ArgumentOutOfRangeException("stride");
                }

                if (bufferSize < 1)
                {
                    throw new ArgumentOutOfRangeException("bufferSize");
                }

                uint minBufferSize = (uint)((sourceRect.Height - 1) * stride) + minStrideInBytes;
                if (bufferSize < minBufferSize)
                {
                    throw new ArgumentOutOfRangeException("bufferSize");
                }
            }

            CopyPixelsCore(sourceRect, stride, bufferSize, buffer);
        }

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="rc">
        ///     The caller can restrict the operation to a rectangle of
        ///     interest (ROI) using this parameter. The ROI sub-rectangle
        ///     must be fully contained in the bounds of the bitmap.
        ///     Specifying a null ROI implies that the whole bitmap should be
        ///     returned. Careful use of the ROI can be a significant
        ///     performance optimization when the pixel-production algorithm
        ///     is expensive - e.g. JPEG decoding. 
        /// </param>
        /// <param name="stride">
        ///     Defines the count of bytes between two vertically adjacent
        ///     pixels in the output buffer. 
        /// </param>
        /// <param name="bufferSize">
        ///     The size
        /// </param>
        /// <param name="buffer">
        ///     The caller controls the memory management and must provide an
        ///     output buffer of sufficient size to complete the call based on
        ///     the width, height and pixel format of the bitmap and the
        ///     sub-rectangle provided to the copy method. 
        /// </param>
        /// <remarks>
        ///     This is the main image processing routine. It instructs the
        ///     BitmapSource instance to produce pixels according to its
        ///     algorithm - this may involve decoding a portion of a JPEG
        ///     stored on disk, copying a block of memory, or even
        ///     analytically computing a complex gradient. The algorithm is
        ///     completely dependent on the implementation.
        ///     
        ///     Implementation of this method must only write to the first
        ///     (rc.Width*PixelFormat.BitsPerPixel+7)/8 bytes of each line of
        ///     the output buffer (in this case, a line is a consecutive string
        ///     of "stride" bytes).
        /// </remarks>
        protected virtual void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
        {
        }

        /// <summary>
        ///     Get the size of the specified array and of the elements in it.
        /// </summary>
        /// <param name="pixels">
        ///     The array to get info about.
        /// </param>
        /// <param name="elementSize">
        ///     On output, will contain the size of the elements in the array.
        /// </param>
        /// <param name="sourceBufferSize">
        ///     On output, will contain the size of the array.
        /// </param>
        /// <param name="elementType">
        ///     On output, will contain the type of the elements in the array.
        /// </param>
        private void ValidateArrayAndGetInfo(Array pixels,
                                             out int elementSize,
                                             out int sourceBufferSize,
                                             out Type elementType)
        {
            //
            // Assure that a valid pixels Array was provided.
            //
            if (pixels == null)
            {
                throw new ArgumentNullException("pixels");
            }

            if (pixels.Rank == 1)
            {
                if (pixels.GetLength(0) <= 0)
                {
                    throw new ArgumentException("pixels");
                }
                else
                {
                    checked
                    {
                        object exemplar = pixels.GetValue(0);
                        elementSize = Marshal.SizeOf(exemplar);
                        sourceBufferSize = pixels.GetLength(0) * elementSize;
                        elementType = exemplar.GetType();
                    }
                }

            }
            else if (pixels.Rank == 2)
            {
                if (pixels.GetLength(0) <= 0 || pixels.GetLength(1) <= 0)
                {
                    throw new ArgumentException("pixels");
                }
                else
                {
                    checked
                    {
                        object exemplar = pixels.GetValue(0, 0);
                        elementSize = Marshal.SizeOf(exemplar);
                        sourceBufferSize = pixels.GetLength(0) * pixels.GetLength(1) * elementSize;
                        elementType = exemplar.GetType();
                    }
                }
            }
            else
            {
                throw new ArgumentException("pixels");
            }
        }
        #endregion BitmapSource CopyPixels

        #region BitmapSource Download

        /// <summary>
        ///     Whether or not the BitmapSource is downloading content.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation return false.
        /// </remarks>
        public override bool IsDownloading
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///     Raised when the downloading of content is completed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation does nothing.
        /// </remarks>
        public override event EventHandler DownloadCompleted
        {
            add
            {
                _downloadCompleted += value;
            }

            remove
            {
                _downloadCompleted -= value;
            }
        }

        /// <summary>
        ///     Raises the dowload completed event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDownloadCompleted()
        {
            EventHandler downloadCompleted = _downloadCompleted;
            if (downloadCompleted != null)
            {
                downloadCompleted(this, EventArgs.Empty);
            }
        }

        private EventHandler _downloadCompleted;

        /// <summary>
        ///     Raised when the downloading of content has progressed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation does nothing.
        /// </remarks>
        public override event EventHandler<DownloadProgressEventArgs> DownloadProgress
        {
            add
            {
                _downloadProgress += value;
            }

            remove
            {
                _downloadProgress -= value;
            }
        }

        /// <summary>
        ///     Raises the dowload progress event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDownloadProgress(DownloadProgressEventArgs e)
        {
            EventHandler<DownloadProgressEventArgs> downloadProgress = _downloadProgress;
            if (downloadProgress != null)
            {
                downloadProgress(this, e);
            }
        }

        private EventHandler<DownloadProgressEventArgs> _downloadProgress;

        /// <summary>
        ///     Raised when the downloading of content has failed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation throws.
        /// </remarks>
        public override event EventHandler<ExceptionEventArgs> DownloadFailed
        {
            add
            {
                _downloadFailed += value;
            }

            remove
            {
                _downloadFailed -= value;
            }
        }

        /// <summary>
        ///     Raises the dowload failed event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDownloadFailed(ExceptionEventArgs e)
        {
            EventHandler<ExceptionEventArgs> downloadFailed = _downloadFailed;
            if (downloadFailed != null)
            {
                downloadFailed(this, e);
            }
        }

        private EventHandler<ExceptionEventArgs> _downloadFailed;

        #endregion BitmapSource Download

        #region BitmapSource Decode

        /// <summary>
        ///     Raised when the downloading of content has progressed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation does nothing.
        /// </remarks>
        public override event EventHandler<ExceptionEventArgs> DecodeFailed
        {
            add
            {
                _decodeFailed += value;
            }

            remove
            {
                _decodeFailed -= value;
            }
        }

        /// <summary>
        ///     Raises the decode failed event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDecodeFailed(ExceptionEventArgs e)
        {
            EventHandler<ExceptionEventArgs> decodeFailed = _decodeFailed;
            if (decodeFailed != null)
            {
                decodeFailed(this, e);
            }
        }

        private EventHandler<ExceptionEventArgs> _decodeFailed;

        #endregion BitmapSource Decode

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // In 4.0, there is a memory leak due to a cycle going through
                // native code that the GC cannot detect.
                //
                // When someone derives from BitmapSource, WPF creates an
                // instance of the ManagedBitmapSource class to implement the
                // actual IWicBitmapSource interface for MIL to use.  WPF uses
                // COM interop (Marshal.GetComInterfaceForObject) to cast the
                // ManagedBitmapSource to the COM interface.  This pins the
                // ManagedBitmapSource behind the reference-counted COM interface,
                // which is then stored in a BitmapSourceSafeMILHandle, which is
                // held by the original BitmapSource instance.  The pinned
                // ManagedBitmapSource also holds a reference to the orginal
                // BitmapSource, thus a cycle is created that cannot be broken
                // by the GC.  It looks like this:
                //
                // CustomBitmapSource --> 
                //   BitmapSourceSafeMILHandle (ref counted) -->
                //     ManagedBitmapSource -->
                //       CustomBitmapSource
                //
                // WPF will fix this in the future.  For now, we will use
                // private reflection to break the cycle explicitly.
                var _wicSource = this.GetType().GetField("_wicSource", BindingFlags.NonPublic | BindingFlags.Instance);
                SafeHandle handle = _wicSource.GetValue(this) as SafeHandle;
                if (handle != null && !handle.IsInvalid)
                {
                    handle.Dispose();
                    _wicSource.SetValue(this, null);
                }
            }
        }
        #endregion
    }
}
