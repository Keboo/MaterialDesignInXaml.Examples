using Microsoft.Win32;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Windows;
using System.Windows.Media;

namespace SkiaSharp.Svg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Extended.Svg.SKSvg Svg { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenOnClick(object sender, RoutedEventArgs e)
        {
            var picker = new OpenFileDialog
            {
                Filter = "SVGs|*.svg|All Files|*"
            };
            if (picker.ShowDialog() == true)
            {
                Svg = new Extended.Svg.SKSvg();
                Svg.Load(picker.FileName);
                SKElement.InvalidateVisual();
            }
        }

        private void SKElement_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear();

            if (Svg == null) return;

            SKImageInfo info = e.Info;

            canvas.Translate(info.Width / 2f, info.Height / 2f);

            SKRect bounds = Svg.ViewBox;
            float xRatio = info.Width / bounds.Width;
            float yRatio = info.Height / bounds.Height;

            float ratio = Math.Min(xRatio, yRatio);

            canvas.Scale(ratio);
            canvas.Translate(-bounds.MidX, -bounds.MidY);

            using (var paint = new SKPaint())
            {
                paint.ColorFilter = SKColorFilter.CreateBlendMode(Colors.Red.ToSKColor(), SKBlendMode.SrcIn);
                canvas.DrawPicture(Svg.Picture, paint);
            }
        }
    }
}
