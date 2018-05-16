using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Bogus;

namespace DataGrid.SmoothScroll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public IList<Person> People { get; }

        public MainWindow()
        {
            Faker<Person> generator = new Faker<Person>()
                .StrictMode(true)
                .RuleFor(x => x.ID, f => f.IndexGlobal)
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.DOB, f => f.Person.DateOfBirth);

            People = generator.Generate(300);

            InitializeComponent();
        }

        private void ScrollToRow(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowId.Text, out int rowId) && rowId > 0 && rowId < People.Count)
            {
                ScrollViewer scrollViewer = FindVisualChild<ScrollViewer>(DataGrid);
                if (scrollViewer == null) return;

                //Making the assumption that all rows are equal height
                var itemHeight = scrollViewer.ExtentHeight / People.Count;
                
                //Attempt to make it the center item in the viewport
                double targetOffset = itemHeight * (rowId + 1) - scrollViewer.ViewportHeight / 2;

                DoubleAnimation verticalAnimation = new DoubleAnimation
                {
                    From = scrollViewer.VerticalOffset,
                    To = targetOffset,
                    Duration = new Duration(TimeSpan.FromMilliseconds(1000)), //Adjust as needed for speed
                    EasingFunction = new SineEase() //Pick whatever easing function you like. List here: https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/easing-functions
                };


                Storyboard storyboard = new Storyboard();

                storyboard.Children.Add(verticalAnimation);
                Storyboard.SetTarget(verticalAnimation, scrollViewer);

                //Using an attached property here since the Storyboard needs a DependencyProperty as a targer, and ScrollViewer.VerticalOffset does not have a DependencyProperty
                Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(ScrollViewerHelper.VerticalOffsetProperty)); 
                storyboard.Begin();
            }
        }

        private static T FindVisualChild<T>(FrameworkElement startingElement) where T : DependencyObject
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(startingElement);

            while (queue.Count > 0)
            {
                DependencyObject element = queue.Dequeue();
                if (element is T typedElement) return typedElement;
                int childCount = VisualTreeHelper.GetChildrenCount(element);
                for (int i = 0; i < childCount; i++)
                {
                    queue.Enqueue(VisualTreeHelper.GetChild(element, i));
                }
            }
            return default(T);
        }
    }
}
