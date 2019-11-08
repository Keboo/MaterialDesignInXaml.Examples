using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Controls;

namespace DialogHost.ChangingContent.CodeBehind
{
    /// <summary>
    /// Interaction logic for CreateAccountControl.xaml
    /// </summary>
    public partial class CreateAccountControl : UserControl
    {
        public MaterialDesignThemes.Wpf.DialogHost DialogHost { get; }

        public CreateAccountControl(MaterialDesignThemes.Wpf.DialogHost dialogHost)
        {
            InitializeComponent();
            DialogHost = dialogHost ?? throw new ArgumentNullException(nameof(dialogHost));
        }

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DialogHost.CurrentSession is DialogSession session)
            {
                session.UpdateContent(new LoginControl(DialogHost));
            }
        }
    }
}
