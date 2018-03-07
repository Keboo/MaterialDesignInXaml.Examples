using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsControls
{
    public partial class MediaPlayer : UserControl
    {
        public MediaPlayer()
        {
            InitializeComponent();
        }

        public string URL
        {
            get
            {
                return axWindowsMediaPlayer1.URL;
            }

            set
            {
                axWindowsMediaPlayer1.URL = value;
            }
        }
    }
}
