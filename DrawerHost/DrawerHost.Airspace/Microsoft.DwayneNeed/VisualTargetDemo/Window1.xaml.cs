using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.DwayneNeed.Controls;
using Microsoft.DwayneNeed.Threading;

namespace VisualTargetDemo
{
    public partial class Window1 : System.Windows.Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Player1.Child = CreateMediaElementOnWorkerThread();
            Player2.Child = CreateMediaElementOnWorkerThread();
            Player3.Child = CreateMediaElementOnWorkerThread();
        }

        private HostVisual CreateMediaElementOnWorkerThread()
        {
            // Create the HostVisual that will "contain" the VisualTarget
            // on the worker thread.
            HostVisual hostVisual = new HostVisual();

            // Spin up a worker thread, and pass it the HostVisual that it
            // should be part of.
            Thread thread = new Thread(new ParameterizedThreadStart(MediaWorkerThread));
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start(hostVisual);

            // Wait for the worker thread to spin up and create the VisualTarget.
            s_event.WaitOne();

            return hostVisual;
        }

        private FrameworkElement CreateMediaElement()
        {
            // Create a MediaElement, and give it some video content.
            MediaElement mediaElement = new MediaElement();
            mediaElement.BeginInit();
            mediaElement.Source = new Uri("http://download.microsoft.com/download/2/C/4/2C433161-F56C-4BAB-BBC5-B8C6F240AFCC/SL_0410_448x256_300kb_2passCBR.wmv?amp;clcid=0x409");
            mediaElement.Width = 200;
            mediaElement.Height = 100;
            mediaElement.EndInit();

            return mediaElement;
        }

        private void MediaWorkerThread(object arg)
        {
            // Create the VisualTargetPresentationSource and then signal the
            // calling thread, so that it can continue without waiting for us.
            HostVisual hostVisual = (HostVisual)arg;
            VisualTargetPresentationSource visualTargetPS = new VisualTargetPresentationSource(hostVisual);
            s_event.Set();

            // Create a MediaElement and use it as the root visual for the
            // VisualTarget.
            visualTargetPS.RootVisual = CreateMediaElement();

            // Run a dispatcher for this worker thread.  This is the central
            // processing loop for WPF.
            System.Windows.Threading.Dispatcher.Run();
        }

        private static AutoResetEvent s_event = new AutoResetEvent(false);
    }
}