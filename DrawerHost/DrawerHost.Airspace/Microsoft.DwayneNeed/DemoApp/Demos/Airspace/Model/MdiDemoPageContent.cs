using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApp.Demos.Airspace.Model
{
    public class MdiDemoPageContent : MdiDemoContent
    {
        public Uri Uri
        {
            get { return _uri; }
            set { SetValue("Uri", ref _uri, value); }
        }

        private Uri _uri;
    }
}
