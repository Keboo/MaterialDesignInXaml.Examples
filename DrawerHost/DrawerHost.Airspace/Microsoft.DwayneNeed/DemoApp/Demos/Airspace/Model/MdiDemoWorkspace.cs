using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DemoApp.Model;
using System.Collections.ObjectModel;

namespace DemoApp.Demos.Airspace.Model
{
    /// <summary>
    ///     An object in the airspace demo data model that contains the
    ///     options and content of a workspace.  This model can be
    ///     saved and loaded from the demo application.
    /// </summary>
    public class MdiDemoWorkspace : ModelBase
    {
        #region Options
        /// <summary>
        ///     The options for the demo, specifying thins like which airspace
        ///     mitigations to use.
        /// </summary>
        public MdiDemoOptions Options
        {
            get 
            {
                return _options;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("Options may not be set to null.");
                }

                SetValue("Options", ref _options, value);
            }
        }

        private MdiDemoOptions _options = new MdiDemoOptions();
        #endregion

        #region Content
        public ObservableCollection<MdiDemoContent> Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The Content collection may not be set to null.");
                }

                SetValue("Content", ref _content, value);
            }
        }

        private ObservableCollection<MdiDemoContent> _content = new ObservableCollection<MdiDemoContent>();
        #endregion
    }
}
