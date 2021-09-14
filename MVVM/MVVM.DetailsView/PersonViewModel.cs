using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using TestData;

namespace MVVM.DetailsView
{
    public class PersonViewModel : ObservableObject
    {
        public Person Person { get; }
        public PersonViewModel(Person person)
        {
            Person = person ?? throw new ArgumentNullException(nameof(person));

            FirstName = person.FirstName;
            LastName = person.LastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool HasChanges => 
            !string.Equals(FirstName, Person.FirstName) ||
            !string.Equals(LastName, Person.LastName);
    }

}
