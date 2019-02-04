using AutoDI;
using Microsoft.Extensions.Localization;

namespace Pseudoloc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            FromCodeBehind.Text = GetCodeBehindValue();
        }

        private string GetCodeBehindValue([Dependency] IStringLocalizer stringLocalizer = null)
        {
            LocalizedString localizedString = stringLocalizer.GetString("FromCodeBehind_Text");
            return localizedString.Value;
        }
    }
}
