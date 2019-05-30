using System.Collections.Generic;
using TestData;

namespace ListView.GridViewColors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public IList<Person> People { get; }

        public MainWindow()
        {
            People = Data.GeneratePeople(150);

            InitializeComponent();
        }
    }
}
