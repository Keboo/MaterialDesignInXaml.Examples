using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace OpenCV.RectangleDetector
{
    internal static class MatMixins
    {
        //http://www.pyimagesearch.com/2015/04/06/zero-parameter-automatic-canny-edge-detection-with-python-and-opencv/
        public static Mat AutoCanny(this Mat image, double sigma = 0.33)
        {
            //Assumes single channel 8 bit image.
            double v = image.GetAll<byte>().Median();
            int lower = (int)Math.Max(0, (1.0 - sigma) * v);
            int upper = (int)Math.Min(255, (1.0 + sigma) * v);
            return image.Canny(lower, upper);
        }

        public static IEnumerable<T> GetAll<T>(this Mat mat) where T : struct
        {
            if (mat == null) throw new ArgumentNullException(nameof(mat));

            for (int row = 0; row < mat.Rows; ++row)
            {
                for (int col = 0; col < mat.Cols; ++col)
                {
                    yield return mat.Get<T>(row, col);
                }
            }
        }
    }
}