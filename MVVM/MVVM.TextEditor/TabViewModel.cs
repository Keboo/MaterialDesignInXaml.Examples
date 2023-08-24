using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System.IO;
using System.Threading.Tasks;

namespace MVVM.TextEditor;

public partial class TabViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private TextDocument? _textDocument;

    partial void OnTextDocumentChanged(TextDocument? oldValue, TextDocument? newValue)
    {
        if (oldValue is not null)
        {
            oldValue.Changed -= TextDocumentChanged;
        }
        if (newValue is not null)
        {
            newValue.Changed += TextDocumentChanged;
        }

        void TextDocumentChanged(object? sender, DocumentChangeEventArgs e)
        {
            IsDirty = true;
        }
    }

    [ObservableProperty]
    private IHighlightingDefinition? _highlightingDefinition;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private bool _isDirty;

    public TabViewModel(IMessenger messenger)
    {
        Messenger = messenger;
    }

    private IMessenger Messenger { get; }

    public async Task LoadFileAsync(FileInfo file)
    {
        Title = file.Name;
        HighlightingDefinition = HighlightingManager.Instance.GetDefinitionByExtension(file.Extension);
        TextDocument ??= new();
        TextDocument.FileName = file.FullName;
        using StreamReader reader = file.OpenText();
        TextDocument.Text = await reader.ReadToEndAsync();
        IsDirty = false;
    }

    [RelayCommand]
    public void Close()
    {
        Messenger.Send(new RequestTabCloseMessage(this));
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task Save()
    {
        if (TextDocument is not null)
        {
            using StreamWriter writer = new(TextDocument.FileName);
            await writer.WriteAsync(TextDocument.Text);
            IsDirty = false;
        }
    }

    private bool CanSave() => IsDirty;
}

public record class RequestTabCloseMessage(TabViewModel Tab);
