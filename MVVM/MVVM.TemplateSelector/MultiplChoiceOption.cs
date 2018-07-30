using System.Collections.Generic;

namespace MVVM.TemplateSelector
{
    public class MultiplChoiceOption : Option
    {
        public IList<string> Options { get; }

        public MultiplChoiceOption(string text, string type, IList<string> options)
            : base(text, type)
        {
            Options = options;
        }
    }
}