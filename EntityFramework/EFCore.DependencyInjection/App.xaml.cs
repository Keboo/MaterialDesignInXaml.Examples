using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
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
        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Visibility = Visibility.Visible;
        app.Run();
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
        });
}
