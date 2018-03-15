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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.DwayneNeed.Media.Imaging;
using System.ComponentModel;

namespace CustomBitmapDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            // Work-around for getting notified whenever the text changes in
            // an editable combo box.
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ComboBox.TextProperty, typeof(ComboBox));
            dpd.AddValueChanged(BitmapUriList, OnTextChanged);
        }

        // This comes from changes to the source URI combo box.
        private void OnTextChanged(object sender, EventArgs e)
        {
            RebuildBitmapChain();
        }

        // This comes from changes to any of the combo boxes.
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RebuildBitmapChain();
        }

        private void RebuildBitmapChain()
        {
            if (TheImage != null)
            {
                IDisposable disposable = TheImage.Source as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }

                TheImage.Source = null;
            }

            // First create a BitmapImage to download the source bitmap from the Uri.
            if (BitmapUriList.Text != null && BitmapUriList.Text.Length > 0)
            {
                Uri sourceUri = new Uri(BitmapUriList.Text, UriKind.Absolute);

                BitmapImage bitmap = new BitmapImage(sourceUri);
                if (bitmap.IsDownloading)
                {
                    bitmap.DownloadCompleted += RebuildBitmapChain_DownloadCompleted;
                }
                else
                {
                    // Call DownloadCompleted ourselves to keep going...
                    RebuildBitmapChain_DownloadCompleted(bitmap, EventArgs.Empty);
                }
            }
        }

        private void RebuildBitmapChain_DownloadCompleted(object sender, EventArgs e)
        {
            BitmapSource source = (BitmapSource) sender;
            ChainedBitmap chainedBitmap = null;

            // Create the second BitmapSource in the chain.
            chainedBitmap = CreateChainedBitmapSource(CustomBitmapList1);
            if (chainedBitmap != null)
            {
                chainedBitmap.Source = source;
                source = chainedBitmap;
            }

            // Create the third BitmapSource in the chain.
            chainedBitmap = CreateChainedBitmapSource(CustomBitmapList2);
            if (chainedBitmap != null)
            {
                chainedBitmap.Source = source;
                source = chainedBitmap;
            }

            TheImage.Source = source;
        }

        private ChainedBitmap CreateChainedBitmapSource(ComboBox combobox)
        {
            ComboBoxItem selectedCustomBitmapItem = combobox.SelectedItem as ComboBoxItem;
            string customBitmapName = selectedCustomBitmapItem.Content as string;
            switch (customBitmapName)
            {
                case "Grayscale":
                    return new GrayscaleBitmap();
                case "Sepia":
                    return new SepiaBitmap();
                case "ColorKey":
                    return new ColorKeyBitmap();
                case "None":
                default:
                    return null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

}
