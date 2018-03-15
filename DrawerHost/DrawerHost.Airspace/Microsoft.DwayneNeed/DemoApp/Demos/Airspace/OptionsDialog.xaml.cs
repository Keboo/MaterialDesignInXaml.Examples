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
using System.Windows.Shapes;
using Microsoft.DwayneNeed.Interop;
using System.IO;
using DemoApp.Demos.Airspace.Model;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for OptionsDialog.xaml
    /// </summary>
    public partial class OptionsDialog : Window
    {
        #region Options
        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(
            /* Name:                 */ "Options",
            /* Value Type:           */ typeof(MdiDemoOptions),
            /* Owner Type:           */ typeof(OptionsDialog),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null, // This will be coerced!
            /*     Property Changed: */ null,
            /*     Coerce Vale:      */ (d, baseValue) => ((OptionsDialog)d).Options_CoerceValue(baseValue)));

        /// <summary>
        ///     The options for the demo, specifying thins like which airspace
        ///     mitigations to use.
        /// </summary>
        public MdiDemoOptions Options
        {
            get { return (MdiDemoOptions)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        /// <summary>
        ///     We never allow the MdiDemoOptions property to be null.  If
        ///     a null value is set locally (null is also the default value)
        ///     then we coerce it to a new instance.
        /// </summary>
        /// <param name="baseValue"></param>
        /// <returns></returns>
        private object Options_CoerceValue(object baseValue)
        {
            if (baseValue == null)
            {
                if (Options_DefaultValue == null)
                {
                    Options_DefaultValue = new MdiDemoOptions();
                }

                return Options_DefaultValue;
            }
            else
            {
                // Now that a real instance has been provided (this is not the
                // default value), throw away the old default value.  We will
                // make a new one default value if the property is set back to
                // null.
                Options_DefaultValue = null;

                return baseValue;
            }
        }

        private MdiDemoOptions Options_DefaultValue;
        #endregion

        #region MdiViewBackground
        /// <summary>
        ///     MdiViewBackgroundPropertyKey is a private property
        ///     that will be bound to MdiDemoOptions.MdiViewBackground
        ///     in order to respond whenever that option changes.
        /// </summary>
        public static readonly DependencyProperty MdiViewBackgroundProperty = DependencyProperty.Register(
            /* Name:                 */ "MdiViewBackground",
            /* Value Type:           */ typeof(Brush),
            /* Owner Type:           */ typeof(OptionsDialog),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /* Default Value:        */ Brushes.White,
            /*     Property Changed: */ (PropertyChangedCallback)((d, e) => ((OptionsDialog)d).MdiViewBackground_PropertyChanged(e))));

        public Brush MdiViewBackground
        {
            get { return (Brush)GetValue(MdiViewBackgroundProperty); }
            set { SetValue(MdiViewBackgroundProperty, value); }
        }

        /// <summary>
        ///     Our MdiViewBackground property is bound to
        ///     MdiDemoOptions.MdiViewBackground. When this property changes,
        ///     select the appropriate item in the MdiViewBackgroundCombo.
        /// </summary>
        private void MdiViewBackground_PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Brush newValue = (Brush)e.NewValue;
            Brush oldValue = (Brush)e.OldValue;

            if (newValue != oldValue)
            {
                object itemToSelect = null;

                if (newValue is ImageBrush)
                {
                    BitmapImage bitmapImage = ((ImageBrush)newValue).ImageSource as BitmapImage;
                    if (bitmapImage != null)
                    {
                        itemToSelect = bitmapImage.UriSource.AbsoluteUri;
                    }
                }
                else
                {
                    foreach (object item in MdiViewBackgroundCombo.Items)
                    {
                        TextBlock tbItem = item as TextBlock;
                        if (tbItem != null)
                        {
                            if (tbItem.Background == newValue)
                            {
                                itemToSelect = tbItem;
                                break;
                            }
                        }
                    }
                }

                MdiViewBackgroundCombo.SelectedItem = itemToSelect;
            }
        }

        /// <summary>
        ///     When an item is selected in the MdiViewBackgroundCombo, set
        ///     the MdiDemoOptions.MdiViewBackground value appropriately.
        ///     We may need to download the appropriate bitmap first.
        /// </summary>
        private void MdiViewBackgroundCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedItem = MdiViewBackgroundCombo.SelectedItem;

            if (selectedItem is string)
            {
                Uri url = new Uri((string)selectedItem);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = url;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                if (bitmapImage.IsDownloading)
                {
                    bitmapImage.DownloadCompleted += new EventHandler(bitmapImage_DownloadCompleted);
                }
                else
                {
                    bitmapImage_DownloadCompleted(bitmapImage, EventArgs.Empty);
                }
            }
            else if (selectedItem is TextBlock)
            {
                Options.MdiViewBackground = ((TextBlock)selectedItem).Background;
            }
            else
            {
                Options.MdiViewBackground = Brushes.White;
            }
        }

        /// <summary>
        ///     This handler is called when a bitmap is downloaded.
        ///     We started to download the bitmap when an item in the
        ///     MdiViewBackgroundCombo was selected that referenced a bitmap.
        ///     If that same item is still selected, update the
        ///     MdiDemoOptions.MdiViewBackground to be a brush containing the
        ///     bitmap.
        /// </summary>
        void bitmapImage_DownloadCompleted(object sender, EventArgs e)
        {
            BitmapImage bitmapImage = sender as BitmapImage;
            if (bitmapImage != null)
            {
                bitmapImage.Freeze();

                object selectedItem = MdiViewBackgroundCombo.SelectedItem;
                if (selectedItem is string)
                {
                    Uri url = new Uri((string)selectedItem);
                    if (bitmapImage.UriSource == url)
                    {
                        ImageBrush brush = new ImageBrush(bitmapImage);
                        brush.Freeze();

                        Options.MdiViewBackground = brush;
                    }
                }
            }
        }

        /// <summary>
        ///     Populate the MdiViewBackgroundCombo with items referencing the
        ///     bitmaps available in the specified directory.
        /// </summary>
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
                        MdiViewBackgroundCombo.Items.Add(file.FullName);
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
        #endregion        
        #region HwndSourceHostBackground
        /// <summary>
        ///     HwndSourceHostBackgroundPropertyKey is a private property
        ///     that will be bound to MdiDemoOptions.HwndSourceHostBackground
        ///     in order to respond whenever that option changes.
        /// </summary>
        private static readonly DependencyProperty HwndSourceHostBackgroundProperty = DependencyProperty.Register(
            /* Name:                 */ "HwndSourceHostBackground",
            /* Value Type:           */ typeof(Brush),
            /* Owner Type:           */ typeof(OptionsDialog),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /* Default Value:        */ Brushes.White,
            /*     Property Changed: */ (PropertyChangedCallback)((d, e) => ((OptionsDialog)d).HwndSourceHostBackground_PropertyChanged(e))));

        /// <summary>
        ///     Our HwndSourceHostBackground property is bound to
        ///     MdiDemoOptions.HwndSourceHostBackground. When this property
        ///     changes, select the appropriate item in the
        ///     HwndSourceHostBackgroundCombo.
        /// </summary>
        private void HwndSourceHostBackground_PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Brush newValue = (Brush)e.NewValue;
            Brush oldValue = (Brush)e.OldValue;

            if (newValue != oldValue)
            {
                object itemToSelect = null;

                foreach (object item in HwndSourceHostBackgroundCombo.Items)
                {
                    TextBlock tbItem = item as TextBlock;
                    if (tbItem != null)
                    {
                        if (tbItem.Background == newValue)
                        {
                            itemToSelect = tbItem;
                            break;
                        }
                    }
                }

                HwndSourceHostBackgroundCombo.SelectedItem = itemToSelect;
            }
        }

        /// <summary>
        ///     When an item is selected in the HwndSourceHostBackgroundCombo,
        ///     set the MdiDemoOptions.HwndSourceHostBackground value to the
        ///     appropriate brush.
        /// </summary>
        private void HwndSourceHostBackgroundCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedItem = HwndSourceHostBackgroundCombo.SelectedItem;

            if (selectedItem is TextBlock)
            {
                Options.HwndSourceHostBackground = ((TextBlock)selectedItem).Background;
            }
            else
            {
                Options.HwndSourceHostBackground = Brushes.White;
            }
        }
        #endregion        

        public OptionsDialog()
        {
            // Default values are not normally coerced.  So manually coerce it.
            CoerceValue(OptionsProperty);

            InitializeComponent();

            // Grab any images from the user's documents directory
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            PopulateFromDirectory(new DirectoryInfo(myDocuments));

            // MdiViewBackground := Options.MdiViewBackground
            //
            // This enables us to respond to property changes, even when the
            // entire MdiDemoOptions object is replaced.
            BindingOperations.SetBinding(
                this,
                MdiViewBackgroundProperty,
                new Binding { Source = this, Path = new PropertyPath("Options.MdiViewBackground") });

            // HwndSourceHostBackground := Options.HwndSourceHostBackground
            //
            // This enables us to respond to property changes, even when the
            // entire MdiDemoOptions object is replaced.
            BindingOperations.SetBinding(
                this,
                HwndSourceHostBackgroundProperty,
                new Binding { Source = this, Path = new PropertyPath("Options.HwndSourceHostBackground") });
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
