using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestData;

namespace MVVM.DetailsView
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<Person> People { get; } = new();

        private Person? _selectedPerson;
        public Person? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (SetProperty(ref _selectedPerson, value))
                {
                    if (PersonDetails?.HasChanges == true)
                    {
                        //Note: Async method called here
                        _ = HandlePersonChange(PersonDetails, value);
                        return;
                    }

                    if (value is not null)
                    {
                        PersonDetails = new PersonViewModel(value);
                    }
                    else
                    {
                        PersonDetails = null;
                    }
                }
                
            }
        }

        private PersonViewModel? _personDetails;
        public PersonViewModel? PersonDetails
        {
            get => _personDetails;
            set => SetProperty(ref _personDetails, value);
        }

        public MainWindowViewModel()
        {
            foreach(var person in Data.GeneratePeople(15))
            {
                People.Add(person);
            }
        }

        public async Task HandlePersonChange(PersonViewModel previousPerson, Person? newPerson)
        {
            //ListBox needs the resetting of the selected deferred
            //The yeild here defers the resetting of the selected person
            await Task.Yield();
            _selectedPerson = previousPerson.Person;
            OnPropertyChanged(nameof(SelectedPerson));

            var result = await DialogHost.Show(previousPerson);
            switch(result)
            {
                case true:
                    //Save, apply changes, go to new person
                    //Note: Because Person does not implement INotifyPropertyChanged you wont see the name update in the ListBox.
                    previousPerson.Person.FirstName = previousPerson.FirstName;
                    previousPerson.Person.LastName = previousPerson.LastName;
                    break;
                case false:
                    //Discard, don't apply changes, go to new person
                    break;
                default:
                    //Cancel, do nothing
                    return;
            }
            PersonDetails = null;
            SelectedPerson = newPerson;
        }
    }

}
