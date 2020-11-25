using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using TestData;

namespace EFCore
{
    public class MainWindowViewModel
    {
        public ObservableCollection<Person> People { get; } = new();
        public PeopleContext Context { get; }

        public MainWindowViewModel(PeopleContext context)
        {
            BindingOperations.EnableCollectionSynchronization(People, new object());
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task LoadData()
        {
            await foreach(var person in Context.People.AsAsyncEnumerable().ConfigureAwait(false))
            {
                //Just simulating a slow query
                await Task.Delay(100).ConfigureAwait(false);
                People.Add(person);
            }
        }
    }
}
