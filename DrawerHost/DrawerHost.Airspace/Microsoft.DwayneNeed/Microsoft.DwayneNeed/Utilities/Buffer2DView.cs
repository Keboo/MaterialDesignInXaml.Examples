using Microsoft.DwayneNeed.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Microsoft.DwayneNeed.Utilities
{
    public struct Buffer2DView<T> where T : struct
    {
        public Buffer2DView(Buffer2D<T> buffer)
        {
            _buffer = buffer;
        }
        
        public Buffer2DView(Buffer2D<T> buffer, Int32Rect bounds)
        {
            _buffer = new Buffer2D<T>(buffer, bounds);
        }

        public Buffer2DView(Buffer2DView<T> buffer, Int32Rect bounds)
        {
            _buffer = new Buffer2D<T>(buffer._buffer, bounds);
        }

        public bool CompareBits(Buffer2D<T> srcBuffer, Int32Rect srcRect, Int32Point? dstPoint = null)
        {
            return _buffer.CompareBits(srcBuffer, srcRect, dstPoint);
        }

        public T this[int x, int y] { get { return _buffer[x,y]; } }
        public int Width { get { return _buffer.Width; } }
        public int Height { get { return _buffer.Height; } }

        public BitmapSource CreateBitmapSource(double dpiX, double dpiY, PixelFormat pixelFormat, BitmapPalette bitmapPalette)
        {
            return _buffer.CreateBitmapSource(dpiX, dpiY, pixelFormat, bitmapPalette);
        }

        internal Buffer2D<T> _buffer;
    }
}
