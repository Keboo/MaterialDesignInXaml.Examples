using System;
using System.Windows.Markup;

namespace DialogHost.CommonDialogWindow
{
    [MarkupExtensionReturnType(typeof(string))]
    public class GuidGenerator : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            string rv = Guid.NewGuid().ToString();
            return rv;
        }
    }
}