using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MVVM.TextEditor;

public partial class MainWindowViewModel : ObservableObject, IRecipient<RequestTabCloseMessage>
{
    public ObservableCollection<TabViewModel> Tabs { get; } = new();

    [ObservableProperty]
    private TabViewModel? _selectedTab;

    [ObservableProperty]
    private bool _isDarkTheme;

    partial void OnIsDarkThemeChanged(bool value)
    {
        var helper = new PaletteHelper();
        ITheme theme = helper.GetTheme();
        theme.SetBaseTheme(value ? Theme.Dark : Theme.Light);
        helper.SetTheme(theme);
    }

    private IMessenger Messenger { get; }

    public MainWindowViewModel(IMessenger messenger)
    {
        BindingOperations.EnableCollectionSynchronization(Tabs, new());
        Messenger = messenger;
        messenger.RegisterAll(this);

        IsDarkTheme = new PaletteHelper().GetTheme().GetBaseTheme() == BaseTheme.Dark;
    }

    [RelayCommand]
    public async Task OpenFile()
    {
        if (GetFile() is { } file)
        {
            var tab = new TabViewModel(Messenger)
            {
                Title = "Tab " + Tabs.Count,
            };
            Tabs.Add(tab);
            SelectedTab = tab;
            await tab.LoadFileAsync(file);
        }
    }

    private void CloseTab(TabViewModel tabViewModel)
    {
        Tabs.Remove(tabViewModel);
    }

    private static FileInfo? GetFile()
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = "Open File",
            Filter = "All Files (*.*)|*.*",
            CheckFileExists = true,
            CheckPathExists = true,
            RestoreDirectory = true,
        };
        if (openFileDialog.ShowDialog() == true)
        {
            return new FileInfo(openFileDialog.FileName);
        }
        return null;
    }



    void IRecipient<RequestTabCloseMessage>.Receive(RequestTabCloseMessage message)
    {
        CloseTab(message.Tab);
    }
}
