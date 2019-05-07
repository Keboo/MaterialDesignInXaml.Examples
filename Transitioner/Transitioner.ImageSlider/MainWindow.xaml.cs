using TestData;

namespace Transitioner.ImageSlider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = Data.GenerateImages(8);
            InitializeComponent();
        }
    }
}
