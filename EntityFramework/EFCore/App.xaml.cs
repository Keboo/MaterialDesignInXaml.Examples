using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using TestData;

namespace EFCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using(var context= new PeopleContext())
            {
                context.Database.Migrate();
                // Seed the database
                if (!context.People.Any())
                {
                    foreach(var person in Data.GeneratePeople(20))
                    {
                        person.ID = 0;
                        context.People.Add(person);
                    }
                    context.SaveChanges();
                }
            }

            base.OnStartup(e);
        }
    }
}
