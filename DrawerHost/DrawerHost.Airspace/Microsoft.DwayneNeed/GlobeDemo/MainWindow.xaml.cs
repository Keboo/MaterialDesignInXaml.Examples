using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using Microsoft.DwayneNeed.Media;
using Microsoft.DwayneNeed.Shapes;

namespace GlobeDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _trackball = new Trackball();
            _trackball.EventSource = ViewportPanel;
            TheViewport3D.Camera.Transform = _trackball.Transform;
        }

        private void MeshResolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MeshResolution.SelectedIndex)
            {
                case 0:
                    TheShape.DivU = 8;
                    TheShape.DivV = 8;
                    break;

                case 1:
                    TheShape.DivU = 16;
                    TheShape.DivV = 16;
                    break;

                case 2:
                    TheShape.DivU = 32;
                    TheShape.DivV = 32;
                    break;

                case 3:
                    TheShape.DivU = 64;
                    TheShape.DivV = 64;
                    break;

                case 4:
                default:
                    TheShape.DivU = 128;
                    TheShape.DivV = 128;
                    break;
            }
        }

        private void CacheScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CacheScaleCombo.SelectedIndex)
            {
                case 0:
                    TheShape.CacheScale = null;
                    break;

                case 1:
                    TheShape.CacheScale = CacheScale.Auto;
                    break;

                case 2:
                    TheShape.CacheScale = new CacheScale(0.1);
                    break;

                case 3:
                    TheShape.CacheScale = new CacheScale(1.0);
                    break;

                case 4:
                    TheShape.CacheScale = new CacheScale(2.0);
                    break;

                case 5:
                    TheShape.CacheScale = new CacheScale(4.0);
                    break;

                case 6:
                default:
                    TheShape.CacheScale = new CacheScale(8.0);
                    break;
            }
        }

        private void Overlay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Overlay.SelectedIndex == 0)
            {
                //MapOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                //MapOverlay.Visibility = Visibility.Visible;
            }
        }

        private void Shape_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TheShape != null)
            {
                TheViewport3D.Children.Remove(TheShape);
            }

            switch (Shape.SelectedIndex)
            {
                case 0:
                    TheShape = new Sphere();
                    break;

                case 1:
                    TheShape = new Cone();
                    break;

                case 2:
                    TheShape = new Cylinder();
                    break;

                case 3:
                    TheShape = new Ellipsoid();
                    break;

                case 4:
                    TheShape = new RomanSurface();
                    break;

                case 5:
                    TheShape = new Torus();
                    break;

                case 6:
                    TheShape = new SuperEllipsoid();
                    break;

                case 7:
                    TheShape = new SuperToroid();
                    break;

                case 8:
                    TheShape = new MobiusStrip();
                    break;

                case 9:
                    TheShape = new Figure8Torus();
                    break;

                case 10:
                    TheShape = new BoysSurface();
                    break;

                default:
                    break;
            }

            TextBox tb = new TextBox();
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "Windows Presentation Foundation (WPF) is a next-generation presentation system for building Windows client applications with visually stunning user experiences. The core of WPF is a resolution-independent and vector-based rendering engine that is built to take advantage of modern graphics hardware. WPF extends the core with a comprehensive set of application-development features that include Extensible Application Markup Language (XAML), controls, data binding, layout, 2-D and 3-D graphics, animation, styles, templates, documents, media, text, and typography. WPF is included in the Microsoft .NET Framework, so you can build applications that incorporate other elements of the .NET Framework class library.";

            TheShape.FrontMaterial = new Shape3DMaterial(tb);
            // TheShape.BackMaterial = null;
            
            TheViewport3D.Children.Add(TheShape);
            //Dispatcher.BeginInvoke((Action)UpdateTheCanvas, System.Windows.Threading.DispatcherPriority.Background);
        }

        private void UpdateTheCanvas()
        {
            UIElement e = TheShape.GrabFrontChildElement();
            TheCanvas.Children.Add(e);
        }

        private Trackball _trackball;
        private ParametricShape3D TheShape;
    }
}
