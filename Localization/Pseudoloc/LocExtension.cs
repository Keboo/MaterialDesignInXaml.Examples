using AutoDI;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Pseudoloc
{
    public class LocExtension : MarkupExtension
    {
        public object Fallback { get; set; }

        private string Key { get; }

        public LocExtension(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            object value = GetValue();

            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target &&
                target.TargetProperty is DependencyProperty property)
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFrom(value);
                }
                return Convert.ChangeType(value, property.PropertyType);
            }
            return value;
        }

        private object GetValue([Dependency] IStringLocalizer stringLocalizer = null)
        {
            if (stringLocalizer == null) throw new ArgumentNullException(nameof(stringLocalizer));
            
            LocalizedString localizedString = stringLocalizer[Key];
            return localizedString.ResourceNotFound ? Fallback : localizedString.Value;
        }
    }
}