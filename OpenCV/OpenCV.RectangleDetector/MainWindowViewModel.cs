using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            try
            {
                using (Mat image = new Mat(filePath))
                using (Mat resized = image.Resize(GetTargetSize(image.Size()))) //Scale the image, so we are working with something consistent
                {
                    AddImage(resized);

                    using (Mat gray = resized.CvtColor(ColorConversionCodes.BGR2GRAY)) //Convert to gray scale since we don't want the color data
                    using (Mat blur = gray.GaussianBlur(new Size(5, 5), 0)) //Smooth the image to eliminate noise
                    using (Mat autoCanny = blur.AutoCanny()) //Apply canny edge filter to find edges
                    using (Mat structuringElement = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(9, 9)))
                    {
                        AddImage(autoCanny);
                        //Smooth over small possible breaks in edges
                        Cv2.Dilate(autoCanny, autoCanny, structuringElement);
                        AddImage(autoCanny);
                        Cv2.Erode(autoCanny, autoCanny, structuringElement);
                        AddImage(autoCanny);

                        //Change the RetrievalModes to CComp if you want internal polygons too, not just the outer most one.
                        Point[][] contours = autoCanny.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                        //Draw all of the found polygons for reference
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
                                     let approx = Cv2.ApproxPolyDP(contour, 0.04 * permimiter, true)
                                     where approx.Length == 4 //Rectange
                                     let area = Cv2.ContourArea(approx)
                                     orderby area descending //We are looking for the biggest thing
                                     select approx).Take(3).ToArray(); //Grabbing three just for comparison

                        //Colors the found polygons Green->Yellow->Red to indicate best matches.
                        for (int i = 0; i < found.Length; i++)
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