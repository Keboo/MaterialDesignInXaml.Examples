using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoApp.Model;
using System.Windows;

namespace DemoApp.Demos.Airspace.Model
{
    public class MdiDemoContent : ModelBase
    {
        #region WindowRect
        public Rect WindowRect
        {
            get { return _windowRect; }
            set { SetValue("WindowRect", ref _windowRect, value); }
        }

        private Rect _windowRect = new Rect(0,0,300,200);
        #endregion

        #region Title
        public string Title
        {
            get { return _title; }
            set { SetValue("Title", ref _title, value); }
        }

        private string _title = String.Empty;
        #endregion
    }
}
