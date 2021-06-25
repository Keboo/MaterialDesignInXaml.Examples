using System;
using System.Collections.Generic;
using System.CommandLine.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.CommandLine.WinForms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.SetHighDpiMode(HighDpiMode.SystemAware);
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new Form1());
        //}

        public static int Main(string url, IConsole console)
        {
            if (string.IsNullOrEmpty(url))
            {
                console.Error.WriteLine("URL is required");
                return 1;
            }

            Thread thread = new(new ParameterizedThreadStart(ShowBrowser));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(url);

            thread.Join();
            
            return 0;
        }

        private static void ShowBrowser(object url)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form = new();
            form.Load += Form_Load;
            Application.Run(form);
            
            void Form_Load(object sender, EventArgs e)
            {
                Form form = (Form)sender;
                WebBrowser browser = new()
                {
                    Anchor = AnchorStyles.Left |
                         AnchorStyles.Top |
                         AnchorStyles.Right |
                         AnchorStyles.Bottom
                };
                form.Controls.Add(browser);

                form.Width = 800;
                form.Height = 600;

                browser.Url = new Uri(url?.ToString());
            }
        }
    }
}
