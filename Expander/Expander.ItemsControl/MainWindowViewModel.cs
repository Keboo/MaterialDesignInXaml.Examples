using System.Collections.Generic;
using TestData;

namespace Expander.ItemsControl
{
    public class MainWindowViewModel
    {
        public IList<Person> People { get; } = Data.GeneratePeople(10);

    }
}
