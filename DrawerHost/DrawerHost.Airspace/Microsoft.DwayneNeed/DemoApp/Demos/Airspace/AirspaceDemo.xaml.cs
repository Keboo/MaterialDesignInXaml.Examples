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
using System.Windows.Controls.Primitives;
using Microsoft.DwayneNeed.MDI;
using Microsoft.DwayneNeed.Interop;
using Microsoft.DwayneNeed.Win32.DwmApi;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using DemoApp.Demos.Airspace.Model;
using System.Xaml;
using System.Xml;
using Microsoft.DwayneNeed.Win32;

namespace DemoApp.Demos.Airspace
{
    /// <summary>
    /// Interaction logic for MdiDemo.xaml
    /// </summary>
    public partial class AirspaceDemo : Page
    {
        #region Workspace
        public static readonly DependencyProperty WorkspaceProperty = DependencyProperty.Register(
            /* Name:                 */ "Workspace",
            /* Value Type:           */ typeof(MdiDemoWorkspace),
            /* Owner Type:           */ typeof(AirspaceDemo),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ null, // This will be coerced!
            /*     Property Changed: */ null,
            /*     Coerce Vale:      */ (d, baseValue) => ((AirspaceDemo)d).Workspace_CoerceValue(baseValue)));

        /// <summary>
        ///     The demo worspace being displayed.
        /// </summary>
        public MdiDemoWorkspace Workspace
        {
            get { return (MdiDemoWorkspace)GetValue(WorkspaceProperty); }
            set { SetValue(WorkspaceProperty, value); }
        }

        /// <summary>
        ///     We never allow the Workspace property to be null.  If
        ///     a null value is set locally (null is also the default value)
        ///     then we coerce it to a new instance.
        /// </summary>
        private object Workspace_CoerceValue(object baseValue)
        {
            if (baseValue == null)
            {
                if (Workspace_DefaultValue == null)
                {
                    Workspace_DefaultValue = new MdiDemoWorkspace();
                }

                return Workspace_DefaultValue;
            }
            else
            {
                // Now that a real instance has been provided (this is not the
                // default value), throw away the old default value.  We will
                // make a new one default value if the property is set back to
                // null.
                Workspace_DefaultValue = null;

                return baseValue;
            }
        }

        private MdiDemoWorkspace Workspace_DefaultValue;
        private string _workspaceFile;
        #endregion

        #region IsWebBrowserExGpuAccelerationEnabled
        /// <summary>
        ///     IsWebBrowserExGpuAccelerationEnabledPropertyKey is a private property
        ///     that will be bound to MdiDemoOptions.IsWebBrowserExGpuAccelerationEnabled
        ///     in order to respond whenever that option changes.
        /// </summary>
        private static readonly DependencyProperty IsWebBrowserExGpuAccelerationEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "IsWebBrowserExGpuAccelerationEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(AirspaceDemo),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ MdiDemoOptions.IsWebBrowserExGpuAccelerationEnabled_GetDefaultValue(),
            /*     Property Changed: */ (PropertyChangedCallback)((d, e) => ((AirspaceDemo)d).IsWebBrowserExGpuAccelerationEnabled_PropertyChanged(e))));

        private void IsWebBrowserExGpuAccelerationEnabled_PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;

            if (newValue != oldValue)
            {
                string appName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_GPU_RENDERING");
                key.SetValue(appName, newValue ? 1 : 0, RegistryValueKind.DWord);

                MessageBox.Show("The GPU Acceleration property has changed.  You must restart the application for this to take effect.");
            }
        }
        #endregion
        #region IsDwmDesktopCompositionEnabled
        /// <summary>
        ///     IsDwmDesktopCompositionEnabledPropertyKey is a private property
        ///     that will be bound to MdiDemoOptions.IsDwmDesktopCompositionEnabled
        ///     in order to respond whenever that option changes.
        /// </summary>
        private static readonly DependencyProperty IsDwmDesktopCompositionEnabledProperty = DependencyProperty.Register(
            /* Name:                 */ "IsDwmDesktopCompositionEnabled",
            /* Value Type:           */ typeof(bool),
            /* Owner Type:           */ typeof(AirspaceDemo),
            /* Metadata:             */ new FrameworkPropertyMetadata(
            /*     Default Value:    */ MdiDemoOptions.IsDwmDesktopCompositionEnabled_GetDefaultValue(),
            /*     Property Changed: */ (PropertyChangedCallback)((d, e) => ((AirspaceDemo)d).IsDwmDesktopCompositionEnabled_PropertyChanged(e))));

        private void IsDwmDesktopCompositionEnabled_PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;

            if (newValue != oldValue)
            {
                NativeMethods.DwmEnableComposition(newValue ? DWM_EC.ENABLECOMPOSITION : DWM_EC.DISABLECOMPOSITION);
            }
        }
        #endregion

        #region CloseWindow
        /// <summary>
        ///     CanExecute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        /// <remarks>
        ///     The command is only enabled if the MdiWindow instance is one
        ///     of the containers generated for one of the MdiDemoContent
        ///     instances in Worspace.Content.
        /// </remarks>
        private void CloseWindow_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            MdiWindow window = MainMdiView.ContainerFromElement((DependencyObject)e.OriginalSource);
            if (window != null)
            {
                MdiDemoContent item = MainMdiView.ItemContainerGenerator.ItemFromContainer(window) as MdiDemoContent;
                if (item != null && Workspace.Content.Contains(item))
                {
                    e.CanExecute = true;
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        ///     Execute handler for the MdiCommands.CloseWindow command.
        /// </summary>
        /// <remarks>
        ///     The command is only enabled if the MdiWindow instance is one
        ///     of the containers generated for one of the MdiDemoContent
        ///     instances in Worspace.Content.  When the command is executed,
        ///     the item is removed from the Workspace.Content collection.
        /// </remarks>
        private void CloseWindow_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            MdiWindow window = MainMdiView.ContainerFromElement((DependencyObject)e.OriginalSource);
            if (window != null)
            {
                MdiDemoContent item = MainMdiView.ItemContainerGenerator.ItemFromContainer(window) as MdiDemoContent;
                if (item != null && Workspace.Content.Contains(item))
                {
                    Workspace.Content.Remove(item);
                }
            }
        }
        #endregion
        #region NewWorkspace
        /// <summary>
        ///     CanExecute handler for the MdiDemoCommands.NewWorkspace command.
        /// </summary>
        private void NewWorkspace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        ///     Execute handler for the MdiDemoCommands.NewWorkspace command.
        /// </summary>
        private void NewWorkspace_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Workspace = new MdiDemoWorkspace();
            _workspaceFile = null;
        }
        #endregion
        #region OpenWorkspace
        /// <summary>
        ///     CanExecute handler for the MdiDemoCommands.OpenWorkspace command.
        /// </summary>
        private void OpenWorkspace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        ///     Execute handler for the MdiDemoCommands.OpenWorkspace command.
        /// </summary>
        private void OpenWorkspace_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = ".mdiw";
            dialog.DereferenceLinks = true;
            dialog.Filter = "MDI Workspace Files (*.mdiw)|*.mdiw|All Files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Multiselect = false;
            dialog.ReadOnlyChecked = false;
            dialog.ShowReadOnly = false;
            dialog.Title = "Open Workspace...";
            dialog.ValidateNames = true;
            if (dialog.ShowDialog().GetValueOrDefault() == true)
            {
                LoadWorkspace(dialog.FileName);
            }
        }
        #endregion
        #region SaveWorkspace
        /// <summary>
        ///     CanExecute handler for the MdiDemoCommands.SaveWorkspace command.
        /// </summary>
        private void SaveWorkspace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_workspaceFile != null);
        }

        /// <summary>
        ///     Execute handler for the MdiDemoCommands.SaveWorkspace command.
        /// </summary>
        private void SaveWorkspace_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (_workspaceFile != null)
            {
                SaveWorkspaceAs(_workspaceFile);
            }
        }
        #endregion
        #region SaveWorkspaceAs
        /// <summary>
        ///     CanExecute handler for the MdiDemoCommands.SaveWorkspaceAs command.
        /// </summary>
        private void SaveWorkspaceAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        ///     Execute handler for the MdiDemoCommands.SaveWorkspace command.
        /// </summary>
        private void SaveWorkspaceAs_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.CheckFileExists = false;
            dialog.CheckPathExists = true;
            dialog.CreatePrompt = false;
            dialog.DefaultExt = ".mdiw";
            dialog.DereferenceLinks = true;
            dialog.Filter = "MDI Workspace Files (*.mdiw)|*.mdiw|All Files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Title = "Save Workspace As...";
            dialog.ValidateNames = false;
            if (dialog.ShowDialog().GetValueOrDefault() == true)
            {
                SaveWorkspaceAs(dialog.FileName);
            }
        }

        private void SaveWorkspaceAs(string filename)
        {
            try
            {
                using(XamlObjectReader reader = new XamlObjectReader(Workspace))
                {
                    using(XmlTextWriter xmlTextWriter = new XmlTextWriter(filename, Encoding.Default))
                    {
                        xmlTextWriter.Formatting = Formatting.Indented;

                        using(XamlXmlWriter writer = new XamlXmlWriter(xmlTextWriter, reader.SchemaContext))
                        {
                            while (reader.Read())
                            {
                                writer.WriteNode(reader);
                            }
                        }
                    }
                }

                // All good, update the filename for future saves.
                _workspaceFile = filename;
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured saving the MDI workspace file.");
            }

        }
        #endregion

        private void LoadWorkspace(string workspaceFile)
        {
            try
            {
                using (XamlXmlReader reader = new XamlXmlReader(workspaceFile))
                {
                    using (XamlObjectWriter writer = new XamlObjectWriter(reader.SchemaContext))
                    {
                        while (reader.Read())
                        {
                            writer.WriteNode(reader);
                        }

                        Workspace = (MdiDemoWorkspace)writer.Result;
                        _workspaceFile = workspaceFile;
                    }
                }

                GC.Collect(); // lame!  The XamlXmlReader will leave the file open until the next GC.
                GC.WaitForPendingFinalizers();
            }
            catch (Exception)
            {
                MessageBox.Show(String.Format("An error occured opening MDI workspace file '{0}'.", workspaceFile));
            }
        }

        public AirspaceDemo()
        {
            // Default values are not normally coerced.  So manually coerce it.
            CoerceValue(WorkspaceProperty);

            InitializeComponent();

            // IsWebBrowserExGpuAccelerationEnabled := Workspace.Options.IsWebBrowserExGpuAccelerationEnabled
            //
            // This enables us to respond to property changes, even when the
            // entire MdiDemoOptions object is replaced.
            BindingOperations.SetBinding(
                this,
                IsWebBrowserExGpuAccelerationEnabledProperty,
                new Binding { Source = this, Path = new PropertyPath("Workspace.Options.IsWebBrowserExGpuAccelerationEnabled") });

            // IsDwmDesktopCompositionEnabled := Workspace.Options.IsDwmDesktopCompositionEnabled
            //
            // This enables us to respond to property changes, even when the
            // entire MdiDemoOptions object is replaced.
            BindingOperations.SetBinding(
                this,
                IsDwmDesktopCompositionEnabledProperty,
                new Binding { Source = this, Path = new PropertyPath("Workspace.Options.IsDwmDesktopCompositionEnabled") });

            // Open the Default workspace file
            _workspaceFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Default.mdiw");
            if (File.Exists(_workspaceFile))
            {
                LoadWorkspace(_workspaceFile);
            }
        }


        private void AddContent(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string uriString = menuItem.Tag as string;
            AddContent(uriString);
        }

        private void AddContent(string uriString)
        {
            MdiDemoContent newContent = null;
            switch (uriString)
            {
                case "MdiWindow":
                    newContent = new MdiDemoContent() { Title = "Empty"};
                    break;

                case "Small":
                    newContent = new MdiDemoColorGridContent() { Title = "Small Grid", NumberOfCells = 100 };
                    break;

                case "Medium":
                    newContent = new MdiDemoColorGridContent() { Title = "Medium Grid", NumberOfCells = 1000 };
                    break;
                
                case "Large":
                    newContent = new MdiDemoColorGridContent() { Title = "Large Grid", NumberOfCells = 10000 };
                    break;

                default:
                    newContent = new MdiDemoPageContent() {Title=uriString, Uri=new Uri(uriString, UriKind.Relative)};
                    break;
            }

            if (!(new Rect(MainMdiView.RenderSize).Contains(_nextNewContentPosition)))
            {
                _nextNewContentPosition = new Point(0, 0);
            }

            newContent.WindowRect = new Rect(_nextNewContentPosition, new Size(300, 200));
            _nextNewContentPosition.Offset(20, 20);

            Workspace.Content.Add(newContent);
        }

        private static Point _nextNewContentPosition = new Point(0, 0);

        private void ShowOptions(object sender, RoutedEventArgs e)
        {
            Window owner = Window.GetWindow(this);

            OptionsDialog dlg = new OptionsDialog();
            dlg.Owner = owner;
            dlg.Options = Workspace.Options.MakeCopy();

            bool? result = dlg.ShowDialog();
            if (result.GetValueOrDefault() == true)
            {
                Workspace.Options = dlg.Options;
            }
        }
    }
}
