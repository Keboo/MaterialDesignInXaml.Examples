using System.Collections.Generic;

namespace ListViewColors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public List<User> Users { get; }

        public MainWindow()
        {
            Users = new List<User>
            {
                new User
                {
                    Name = "John Doe",
                    Email = "john@family.com"
                },
                new User
                {
                    Name = "Jane Doe",
                    Email = "jane@family.com"
                },
                new User
                {
                    Name = "Other Guy",
                    Email = "guy@family.com"
                }
            };
            InitializeComponent();
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
