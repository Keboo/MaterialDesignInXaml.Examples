using Jdenticon;
using System;
using System.Collections.Generic;

namespace Transitioner.ImageSlider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var images = new List<Identicon>();
            for (int i = 0; i < 8; i++)
            {
                images.Add(Identicon.FromValue(Guid.NewGuid(), size: 400));
            }

            DataContext = images;
            InitializeComponent();
        }
    }
}
