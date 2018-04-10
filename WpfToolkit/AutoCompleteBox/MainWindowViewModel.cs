using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace AutoCompleteBox
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IList<string> Names { get; }

        private string _SelectedName;
        public string SelectedName
        {
            get => _SelectedName;
            set => Set(ref _SelectedName, value);
        }

        public MainWindowViewModel()
        {
            Names = new[]
            {
                "Jon",
                "Sherril",
                "Ernestina",
                "Shawana",
                "Jan",
                "Heath",
                "Shakira",
                "Bradly",
                "Shala",
                "Sulema",
                "Emma",
                "Kimberley",
                "Marylou",
                "Emery",
                "Luciana",
                "Luanna",
                "Angelika",
                "Lincoln",
                "Romelia",
                "Xenia",
                "Kathyrn",
                "Lanora",
                "Kalyn",
                "Verna",
                "Takisha",
                "Eula",
                "Arvilla",
                "Cristine",
                "Ruthe",
                "Rosann",
                "Tennille",
                "Horace",
                "Winford",
                "Gregoria",
                "Cammie",
                "Bessie",
                "Marylouise",
                "Florance",
                "Min",
                "Sheilah",
                "Reginald",
                "Rachael",
                "Jule",
                "Larisa",
                "Gita",
                "Dee",
                "Felipe",
                "Georgene",
                "Garrett",
                "Majorie",
            };
        }
    }
}