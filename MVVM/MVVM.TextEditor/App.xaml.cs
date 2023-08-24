using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MVVM.TextEditor;

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
        .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
            => configurationBuilder.AddUserSecrets(typeof(App).Assembly))
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<WeakReferenceMessenger>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

            services.AddSingleton(_ => Current.Dispatcher);

            services.AddTransient<ISnackbarMessageQueue>(provider =>
            {
                Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
                return new SnackbarMessageQueue(TimeSpan.FromSeconds(3.0), dispatcher);
            });
        });

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        PaletteHelper helper = new();
        if (helper.GetThemeManager() is { } themeManager)
        {
            themeManager.ThemeChanged += ThemeManager_ThemeChanged;
        }
        SetAdditionalThemeBrushes(helper.GetTheme());
    }

    private void ThemeManager_ThemeChanged(object? sender, ThemeChangedEventArgs e)
    {
        SetAdditionalThemeBrushes(e.NewTheme);
    }

    private void SetAdditionalThemeBrushes(ITheme theme)
    {
        SetBrush(SystemColors.WindowBrushKey, theme.Paper);
        SetBrush(SystemColors.WindowTextBrushKey, theme.Body);

        void SetBrush(object key, Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            Resources[key] = brush;
        }
    }

}
