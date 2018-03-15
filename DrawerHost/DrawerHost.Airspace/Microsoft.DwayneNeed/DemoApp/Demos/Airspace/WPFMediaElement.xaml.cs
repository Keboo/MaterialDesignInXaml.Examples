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
    /// Interaction logic for WebBrowserContent.xaml
    /// </summary>
    public partial class WPFMediaElement : Grid
    {
        public WPFMediaElement()
        {
            InitializeComponent();

            // Grab any images from the user's documents directory
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            PopulateFromDirectory(new DirectoryInfo(myDocuments));
            Address.SelectedIndex = 0;
        }

        private void GoClicked(object sender, RoutedEventArgs e)
        {
            if (Address != null)
            {
                string address = Address.Text;
                if (!String.IsNullOrWhiteSpace(address))
                {
                    Play(address);
                }
            }
        }

        private void AddressSelectionChanged(object sender, RoutedEventArgs e)
        {
            string address = Address.SelectedValue as string;
            if (address != null)
            {
                Play(address);
            }
        }

        private void Play(string address)
        {
            Player.Source = new Uri(address);
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
                    // Only list *.wmv, *.avi, and *.mpg files
                    if (file.FullName.EndsWith(".wmv") || file.FullName.EndsWith(".avi") || file.FullName.EndsWith(".mpg"))
                    {
                        Address.Items.Add(file.FullName);
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
