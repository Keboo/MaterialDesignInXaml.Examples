using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;

namespace SkiaSharp.ImageDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private FileInfo SelectedFile { get; set; }
        private Point? Offset { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void SKElement_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();

            //TODO: bad check, should handle exception
            if (SelectedFile?.Exists != true) return;

            var element = (SKElement)sender;

            //TODO: Horribly inefficient
            using (Stream imageStream = SelectedFile.OpenRead())
            using (SKBitmap bitmap = SKBitmap.Decode(imageStream))
            using (SKPaint paint = new SKPaint())
            using (SKImageFilter filter = GetImageFilters().FirstOrDefault())
            {
                paint.ImageFilter = filter;
                float x = 0, y = 0;
                if (Offset != null)
                {
                    x = (float)Offset.Value.X;
                    y = (float)Offset.Value.Y;
                }

                var source = new SKRect(x, y, x + (float)element.ActualWidth, y + (float)element.ActualHeight);
                canvas.DrawBitmap(bitmap, source, new SKRect(0, 0, (float)element.ActualWidth, (float)element.ActualHeight), paint);
            }

            IEnumerable<SKImageFilter> GetImageFilters()
            {
                if (int.TryParse(Blur.Text, out int blurValue) && blurValue > 0)
                {
                    yield return SKImageFilter.CreateBlur(blurValue, blurValue);
                }
            }
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            var picker = new OpenFileDialog();
            if (picker.ShowDialog() == true)
            {
                SelectedFile = new FileInfo(picker.FileName);
                SKElement.InvalidateVisual();
            }
        }

        private void OnBlurChanged(object sender, TextChangedEventArgs e)
        {
            SKElement.InvalidateVisual();
        }

        private void OnOffsetChanged(object sender, TextChangedEventArgs e)
        {
            string text = OffsetBox.Text;
            var parts = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 &&
                double.TryParse(parts[0], out double xValue) &&
                double.TryParse(parts[1], out double yValue))
            {
                Offset = new Point(xValue, yValue);
            }
            else
            {
                Offset = null;
            }
            SKElement.InvalidateVisual();
        }
    }
}
