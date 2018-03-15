using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DwayneNeed.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Data;

namespace Microsoft.DwayneNeed.Threading
{
    /// <summary>
    ///     The root element of a tree created on the UIThreadPool.
    /// </summary>
    [ContentProperty("Content")]
    public class UIThreadPoolRoot : VisualWrapper<HostVisual>, IDisposable
    {
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content",
                                        typeof(DataTemplate),
                                        typeof(UIThreadPoolRoot),
                                        new UIPropertyMetadata(null, OnContentChangedThunk));

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(UIThreadPoolRoot), new UIPropertyMetadata(null, OnPropertyNameChangedThunk));

        static UIThreadPoolRoot()
        {
            DataContextProperty.OverrideMetadata(typeof(UIThreadPoolRoot), new FrameworkPropertyMetadata(OnDataContextChangedThunk));
        }

        public UIThreadPoolRoot()
        {
            // The UIThreadPoolRoot instance itself is owned by the calling
            // thread, which is typically the UI thread.  It uses a HostVisual
            // as its only child to host the results of inflating the template
            // on a thread pool thread.
            HostVisual child = new HostVisual();
            Child = child;

            _threadPoolThread = UIThreadPool.AcquireThread();

            _threadPoolThread.Dispatcher.Invoke((Action)delegate
            {
                _root = new VisualTargetPresentationSource(child);
                _root.SizeChanged += VisualTargetSizeChanged;
            });
        }

        public void Dispose()
        {
        }

        public DataTemplate Content
        {
            get { return (DataTemplate)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // HACK!  Called on UI thread
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Called by UI thread
        private void OnContentChanged(DependencyPropertyChangedEventArgs e)
        {
            // Seal the data template, so that we can expand it on the worker thread.
            var dataTemplate = e.NewValue as DataTemplate;

            // Asynchronously pass to worker thread.
            _root.Dispatcher.BeginInvoke((Action) delegate
            {
                if (dataTemplate != null)
                {
                    var rootElement = dataTemplate.LoadContent() as FrameworkElement;
                    _root.RootVisual = rootElement;
                    VisualTargetSizeChanged(rootElement.RenderSize);
                }
                else
                {
                    _root.RootVisual = null;
                    VisualTargetSizeChanged(Size.Empty);
                }
            });
        }

        // Called by UI thread
        private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
        {
            var dataContext = e.NewValue;

            // Asynchronously pass to worker thread.
            _root.Dispatcher.BeginInvoke((Action)delegate
            {
                _root.DataContext = dataContext;
            });
        }

        // Called by UI thread
        private void OnPropertyNameChanged(DependencyPropertyChangedEventArgs e)
        {
            var propertyName = e.NewValue as string;

            // Asynchronously pass to worker thread.
            _root.Dispatcher.BeginInvoke((Action)delegate
            {
                // HACK
                _root.PropertyName = propertyName;
            });
        }

        private static void OnContentChangedThunk(Object sender, DependencyPropertyChangedEventArgs e)
        {
            var _this = sender as UIThreadPoolRoot;
            _this.OnContentChanged(e);
        }

        private static void OnDataContextChangedThunk(Object sender, DependencyPropertyChangedEventArgs e)
        {
            var _this = sender as UIThreadPoolRoot;
            _this.OnDataContextChanged(e);
        }

        private static void OnPropertyNameChangedThunk(object sender, DependencyPropertyChangedEventArgs e)
        {
            var _this = sender as UIThreadPoolRoot;
            _this.OnPropertyNameChanged(e);
        }

        // Called by worker thread.
        private void VisualTargetSizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualTargetSizeChanged(e.NewSize);
        }

        // Called by worker thread.
        private void VisualTargetSizeChanged(Size newSize)
        {
            // Asynchronously pass new size over to UI thread.
            this.Dispatcher.BeginInvoke((DispatcherOperationCallback)
                delegate(object parameter)
                {
                    UpdateSize((Size)parameter);
                    return null;
                },
                newSize);
        }

        // Called by UI thread.
        private void UpdateSize(Size newSize)
        {
            this.Width = newSize.Width;
            this.Height = newSize.Height;
        }

        private UIThreadPoolThread _threadPoolThread;
        private VisualTargetPresentationSource _root; // only touch from thread pool thread!
    }
}
