using EFCore.DependencyInjection.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EFCore.DependencyInjection;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread]
    public static void Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        host.Start();

        App app = new();
        app.InitializeComponent();

        using (var scope = host.Services.CreateScope())
        using (var dbContext = scope.ServiceProvider.GetRequiredService<CustomerContext>())
        {
            dbContext.Database.Migrate();

            Task.Run(async () => await PopulateDatabaseAsync(dbContext)).Wait();
        }

        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
    }

    private static async Task PopulateDatabaseAsync(CustomerContext context)
    {
        if (await context.Customers.AnyAsync())
        {
            return;
        }

        foreach(var p in TestData.Data.GeneratePeople(200))
        {
            Person person = new()
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DOB
            };
            person.Orders = new List<Order>
            {
                new Order
                {
                    SubmittedAt = p.DOB
                },
                new Order(),
            };
            context.Customers.Add(person);
        }
        context.SaveChanges();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
        {

        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<WeakReferenceMessenger>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

            services.AddSingleton<Dispatcher>(_ => Current.Dispatcher);

            services.AddDbContext<CustomerContext>(
                options =>
                {
                    options.UseSqlite("Data Source=customers.db");
                    options.UseLazyLoadingProxies(); 
                });
        });
}
