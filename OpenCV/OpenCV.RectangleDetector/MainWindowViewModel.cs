using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp.WpfExtensions;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace OpenCV.RectangleDetector
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ImageSource> Images { get; } = new ObservableCollection<ImageSource>();

        public ICommand OpenImageCommand { get; }

        public MainWindowViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(Images, new object());

            OpenImageCommand = new RelayCommand(OnOpenImage);
        }

        private void OnOpenImage()
        {
            var dialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                Title = "Select Image"
            };

            if (dialog.ShowDialog() == true)
            {
                Task.Run(() => LoadImage(dialog.FileName));
            }
        }

        private void LoadImage(string filePath)
        {
            //using (Mat mat1 = Mat.Zeros(2048, 1536, MatType.CV_8UC1))
            //using (Mat mat2 = Mat.Zeros(2048, 1536, MatType.CV_8UC1))
            //using (var intersection = new Mat(2048, 1536, MatType.CV_8UC1))
            //using (var union = new Mat(2048, 1536, MatType.CV_8UC1))
            //{
            //    mat1.FillPoly(new[] { new[]
            //    {
            //        new Point(565, 267),
            //        new Point(1210, 207),
            //        new Point(1275, 1720),
            //        new Point(568, 1688)
            //    } }, Scalar.All(255));
            //    AddImage(mat1);
            //    mat2.FillPoly(new[] { new[]
            //    {
            //        new Point(564, 268),
            //        new Point(1208, 208),
            //        new Point(1272, 1716),
            //        new Point(572, 1688)
            //    } }, Scalar.All(255));
            //    AddImage(mat2);
            //    Cv2.BitwiseAnd(mat1, mat2, intersection);
            //    int intersectionPixels = Cv2.CountNonZero(intersection);
            //    AddImage(intersection);
            //    Cv2.BitwiseOr(mat1, mat2, union);
            //    int unionPixels = Cv2.CountNonZero(union);
            //    AddImage(union);
            //    double iou = (double) intersectionPixels / unionPixels;
            //}
            //
            //return;

            try
            {
                using (Mat image = new Mat(filePath))
                using (Mat resized = image.Resize(GetTargetSize(image.Size()))) //Scale the image, so we are working with something consistent
                {
                    AddImage(resized);

                    using (Mat gray = resized.CvtColor(ColorConversionCodes.BGRA2GRAY)) //Convert to gray scale since we don't want the color data
                    using (Mat blur = gray.GaussianBlur(new Size(5, 5), 0, borderType:BorderTypes.Replicate)) //Smooth the image to eliminate noise
                    using (Mat autoCanny = blur.AutoCanny(0.75)) //Apply canny edge filter to find edges
                    {
                        AddImage(blur);
                        AddImage(autoCanny);

                        Point[][] contours = autoCanny.FindContoursAsArray(RetrievalModes.List, ContourApproximationModes.ApproxSimple);

                        //Just get the external hull of the contours
                        for (int i = 0; i < contours.Length; i++)
                        {
                            contours[i] = Cv2.ConvexHull(contours[i]);
                        }

                        //Draw all of the found polygons. This is just for reference
                        using (Mat allFound = resized.Clone())
                        {
                            for (int i = 0; i < contours.Length; i++)
                            {
                                Cv2.DrawContours(allFound, contours, i, Scalar.RandomColor(), 2);
                            }
                            AddImage(allFound);
                        }
                        
                        //Find the largest polygons that four corners
                        var found = (from contour in contours
                                     let permimiter = Cv2.ArcLength(contour, true)
                                     let approx = Cv2.ApproxPolyDP(contour, 0.02 * permimiter, true)
                                     where IsValidRectangle(approx, 0.2)
                                     let area = Cv2.ContourArea(contour)
                                     orderby area descending //We are looking for the biggest thing
                                     select contour).Take(3).ToArray(); //Grabbing three just for comparison

                        //Colors the found polygons Green->Yellow->Red to indicate best matches.
                        for (int i = found.Length - 1; i >= 0; i--)
                        {
                            Scalar color;
                            switch (i)
                            {
                                case 0:
                                    color = Scalar.Green;
                                    break;
                                case 1:
                                    color = Scalar.Yellow;
                                    break;
                                case 2:
                                    color = Scalar.Red;
                                    break;
                                default:
                                    color = Scalar.RandomColor();
                                    break;
                            }

                            resized.DrawContours(found, i, color, 3);
                        }
                        AddImage(resized);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            bool IsValidRectangle(Point[] contour, double minimum)
            {
                if (contour.Length != 4) return false;
                double side1 = GetLength(contour[0], contour[1]);
                double side2 = GetLength(contour[1], contour[2]);
                double side3 = GetLength(contour[2], contour[3]);
                double side4 = GetLength(contour[3], contour[0]);

                if (Math.Abs(side1 - side3) / Math.Max(side1, side3) > minimum) return false;
                if (Math.Abs(side2 - side4) / Math.Max(side2, side4) > minimum) return false;

                return true;

                double GetLength(Point p1, Point p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
            }

            Size GetTargetSize(Size size, int longSize = 512)
            {
                if (size.Width > size.Height)
                {
                    return new Size(longSize, (int)(longSize * (double)size.Height / size.Width));
                }
                return new Size((int)(longSize * (double)size.Width / size.Height), longSize);
            }
        }

        private void AddImage(Mat image)
        {
            BitmapSource imageSource = image.ToBitmapSource();
            imageSource.Freeze();
            Images.Add(imageSource);
        }
    }
}