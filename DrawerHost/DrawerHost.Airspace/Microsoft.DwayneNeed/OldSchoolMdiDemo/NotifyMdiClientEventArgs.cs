using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interop;

namespace OldSchoolMdiDemo
{
    public class NotifyMdiClientEventArgs : EventArgs
    {
        public NotifyMdiClientEventArgs(ClassicMdiClient client)
        {
            Client = client;
        }

        public ClassicMdiClient Client { get; private set; }
    }
}
