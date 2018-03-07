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
using System.IO;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for WPFImage.xaml
    /// </summary>
    public partial class WPFImage : Grid
    {
        public WPFImage()
        {
            InitializeComponent();

            // Grab any images from the user's documents directory
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            PopulateFromDirectory(new DirectoryInfo(myDocuments));
            ImageUrl.SelectedIndex = 0;
        }

        private void GoClicked(object sender, RoutedEventArgs e)
        {
            if (ImageUrl != null)
            {
                string imageUrl = ImageUrl.Text;
                if (!String.IsNullOrWhiteSpace(imageUrl))
                {
                    Load(imageUrl);
                }
            }
        }

        private void ImageUrl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string imageUrl = ImageUrl.SelectedValue as string;
            if (imageUrl != null)
            {
                Load(imageUrl);
            }
        }

        private void Load(string imageUrl)
        {
            TheImage.Source = new BitmapImage(new Uri(imageUrl));
        }

        private void PopulateFromDirectory(DirectoryInfo dir)
        {
            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    PopulateFromDirectory(subDir);
                }

                foreach (FileInfo file in dir.GetFiles())
                {
                    // Only list *.png, *.jpg, and *.bmp files
                    if (file.FullName.EndsWith(".png") || file.FullName.EndsWith(".jpg") || file.FullName.EndsWith(".bmp"))
                    {
                        ImageUrl.Items.Add(file.FullName);
                    }
                }
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

    }
}
