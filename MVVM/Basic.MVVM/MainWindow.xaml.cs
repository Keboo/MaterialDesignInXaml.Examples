namespace Basic.MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var viewModel = new ViewModel();
            viewModel.FirstName = "Kevin";

            DataContext = viewModel;
            InitializeComponent();

            viewModel.FirstName = "Mark";
        }
    }
}
