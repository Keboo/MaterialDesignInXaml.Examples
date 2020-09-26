using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TestData;

namespace DataGrid.SelectedRow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private List<PersonViewModel> SelectedPeople { get; } = new List<PersonViewModel>();

        public MainWindow()
        {
            InitializeComponent();
            var people = Data.GeneratePeople(100).Select(x => new PersonViewModel(x)).ToList();
            DataGrid.ItemsSource = people;
            foreach(var person in people)
            {
                person.PropertyChanged += Person_PropertyChanged;
            }
        }

        private void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(PersonViewModel.IsSelected)) return;

            var person = (PersonViewModel)sender;
            if (person.IsSelected)
            {
                SelectedPeople.Add(person);
            }
            else
            {
                SelectedPeople.Remove(person);
            }
            SelectedPeopleTextBlock.Text = string.Join(", ", SelectedPeople.Select(p => $"{p.FirstName} {p.LastName}"));
        }

        public class PersonViewModel : Person, INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public PersonViewModel(Person person)
            {
                FirstName = person.FirstName;
                LastName = person.LastName;
                DOB = person.DOB;
                ID = person.ID;
            }

            private bool _isSelected;
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    if (value != _isSelected)
                    {
                        _isSelected = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                    }
                }
            }
        }
    }
}
