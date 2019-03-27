using System.Collections;
using System.Collections.Generic;

namespace Transitioner.DataBoundSlides
{
    public class MainWindowViewModel
    {
        public IList<int> Slides { get; } = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7
        };

        public int SelectedIndex { get; set; }
    }
}