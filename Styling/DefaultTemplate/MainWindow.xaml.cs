using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace DefaultTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var types = typeof(FrameworkElement).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.IsSubclassOf(typeof(FrameworkElement)))
                .OrderBy(x => x.Name)
                .ToArray();
            ComboBox.ItemsSource = types;
            ComboBox.DisplayMemberPath = nameof(Type.Name);
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboBox.SelectedItem is Type type)
            {
                TextEditor.Text = GetDefaultTemplate(type) ?? "Not Found";
            }
        }

        /// <summary>
        /// Based on example from: https://www.manuelmeyer.net/2014/08/wpf-the-simplest-way-to-get-the-default-template-of-a-control-as-xaml/
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        private static string? GetDefaultTemplate(Type controlType)
        {
            var control = Application.Current.TryFindResource(controlType);
            if (control is null) return null;
            using MemoryStream ms = new();
            using XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            XamlWriter.Save(control, writer);
            ms.Position = 0;
            using var reader = new StreamReader(ms);
            return reader.ReadToEnd();
        }
    }
}
