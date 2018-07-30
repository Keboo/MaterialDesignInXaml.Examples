using GalaSoft.MvvmLight;

namespace MVVM.TemplateSelector
{
    public class Option : ObservableObject
    {
        public string Text { get; }

        public string Type { get; }

        private object _Value;
        public object Value
        {
            get => _Value;
            set => Set(ref _Value, value);
        }

        public Option(string text, string type)
        {
            Text = text;
            Type = type;
        }
    }
}