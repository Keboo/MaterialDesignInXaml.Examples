using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace MVVM.TemplateSelector
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Option> Options { get; }

        public MainWindowViewModel()
        {
            Options = new ObservableCollection<Option>
            {
                new Option("Do you want large size?", "CheckBox"),
                new Option("My Placeholder", "TextBox"),
                new MultiplChoiceOption("Fruit", "ComboBox", new[] {"Apple", "Kiwi", "Orange"})
            };
        }
    }
}