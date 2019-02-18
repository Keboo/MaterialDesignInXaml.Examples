using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TestData;

namespace ComboBox.CustomFilter
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ObservableCollection<Person> _People;
        public ICollectionView Items { get; }

        private string _TextSearch;
        public string TextSearch
        {
            get => _TextSearch;
            set
            {
                if (Set(ref _TextSearch, value))
                {
                    Items.Refresh();
                }
            }
        }

        public MainWindowViewModel()
        {
            _People = new ObservableCollection<Person>(Data.GeneratePeople(200));
            //This ListCollectionView cast works because we are passing an IList implementation. 
            //Will fail if the passed collection is onlyan ICollection or IEnumerable
            var lv = (ListCollectionView)CollectionViewSource.GetDefaultView(_People);
            Items = lv;

            //Setup a custom sort
            lv.CustomSort = Comparer<Person>.Create(PersonSort);

            //A simple sort and filter setup
            //Sort by last name, then by first name
            //Items.SortDescriptions.Add(new SortDescription(nameof(Person.LastName), ListSortDirection.Ascending));
            //Items.SortDescriptions.Add(new SortDescription(nameof(Person.FirstName), ListSortDirection.Ascending));

            //This adds a filter
            //Items.Filter = PeopleFilter;
        }

        private int PersonSort(Person x, Person y)
        {
            return GetDistance(x).CompareTo(GetDistance(y));
        }

        private bool PeopleFilter(object obj)
        {
            if (string.IsNullOrWhiteSpace(TextSearch))
            {
                return true;
            }
            if (obj is Person person)
            {
                //Could also modify this to use the levinstein distance below
                return person.FirstName.StartsWith(TextSearch, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private int GetDistance(Person person)
        {
            if (string.IsNullOrWhiteSpace(TextSearch))
            {
                return 0;
            }

            string personString = $"{person.FirstName} {person.LastName}";
            //You probably want to improve on this matching.
            //Perhaps something like: 
            //personString = personString.Substring(0, Math.Min(personString.Length, TextSearch.Length));

            return GetDistance(personString, TextSearch);
        }

        //Taken from: https://github.com/dotnet/command-line-api/blob/master/src/System.CommandLine/Invocation/TypoCorrection.cs
        private static int GetDistance(string first, string second)
        {
            // Validate parameters
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }


            // Get the length of both.  If either is 0, return
            // the length of the other, since that number of insertions
            // would be required.

            int n = first.Length, m = second.Length;
            if (n == 0) return m;
            if (m == 0) return n;


            // Rather than maintain an entire matrix (which would require O(n*m) space),
            // just store the current row and the next row, each of which has a length m+1,
            // so just O(m) space. Initialize the current row.

            int curRow = 0, nextRow = 1;
            int[][] rows = { new int[m + 1], new int[m + 1] };

            for (int j = 0; j <= m; ++j)
            {
                rows[curRow][j] = j;
            }

            // For each virtual row (since we only have physical storage for two)
            for (int i = 1; i <= n; ++i)
            {
                // Fill in the values in the row
                rows[nextRow][0] = i;
                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] + (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }


                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            // Return the computed edit distance
            return rows[curRow][m];

        }
    }
}
