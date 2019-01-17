namespace DialogHost.CommonDialogWindow
{
    /// <summary>
    /// Interaction logic for OtherWindow.xaml
    /// </summary>
    public partial class OtherWindow 
    {
        public OtherWindow()
        {
            InitializeComponent();
            DataContext = new OtherWindowViewModel();
        }
    }
}
