using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DemoApp.Model
{
    [ContentProperty("Topics")]
    public class Index : ModelBase
    {
        public ObservableCollection<Topic> Topics { get { return _topics; } }

        private ObservableCollection<Topic> _topics = new ObservableCollection<Topic>();
    }
}
