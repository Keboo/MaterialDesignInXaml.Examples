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
using System.Windows.Forms.Integration;
using System.IO;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for WinFormsMediaPlayer.xaml
    /// </summary>
    public partial class WinFormsMediaPlayer : Grid
    {
        public WinFormsMediaPlayer()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            WindowsFormsHost.EnableWindowsFormsInterop();

            InitializeComponent();

            TheHost.PropertyMap.Remove("Background");

            // Grab any videos from the user's video directory
            string myVideos = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            PopulateFromDirectory(new DirectoryInfo(myVideos));

            // Grab any videos from the public video directory
            string publicVideos = Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos);
            PopulateFromDirectory(new DirectoryInfo(publicVideos));

            TheAddress.SelectedIndex = 0;
        }

        private void PopulateFromDirectory(DirectoryInfo dir)
        {
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                PopulateFromDirectory(subDir);
            }

            foreach (FileInfo file in dir.GetFiles())
            {
                // ignore *.ini files
                if (file.FullName.EndsWith(".ini"))
                {
                    continue;
                }

                TheAddress.Items.Add(file.FullName);
            }

        }

        private void GoClicked(object sender, RoutedEventArgs e)
        {
            if (TheAddress != null)
            {
                string address = TheAddress.Text;
                if (!String.IsNullOrWhiteSpace(address))
                {
                    Play(address);
                }
            }
        }

        private void AddressSelectionChanged(object sender, RoutedEventArgs e)
        {
            string address = TheAddress.SelectedValue as string;
            if (address != null)
            {
                Play(address);
            }
        }

        private void Play(string address)
        {
            TheMediaPlayer.URL = address;
        }
    }
}
