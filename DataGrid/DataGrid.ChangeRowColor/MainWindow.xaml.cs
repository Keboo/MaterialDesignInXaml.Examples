using System.Collections.Generic;
using System.Windows;
using Bogus;

namespace DataGrid.ChangeRowColor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IList<Person> People { get; }

        public MainWindow()
        {
            Faker<Person> generator = new Faker<Person>()
                .StrictMode(true)
                .RuleFor(x => x.ID, f => f.IndexGlobal)
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .RuleFor(x => x.DOB, f => f.Person.DateOfBirth);

            People = generator.Generate(300);

            InitializeComponent();
        }
    }
}
