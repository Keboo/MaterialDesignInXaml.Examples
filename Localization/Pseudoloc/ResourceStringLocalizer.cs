using Microsoft.Extensions.Localization;
using Pseudoloc.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Pseudoloc
{
    public class ResourceStringLocalizer : IStringLocalizer
    {
        private readonly CultureInfo _CultureInfo;

        public ResourceStringLocalizer(CultureInfo cultureInfo = null)
        {
            _CultureInfo = cultureInfo ?? CultureInfo.CurrentUICulture;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new ResourceStringLocalizer(culture);
        }

        public LocalizedString this[string name]
        {
            get
            {
                string value = Resources.ResourceManager.GetString(name, _CultureInfo);
                return new LocalizedString(name, value, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => this[name];
    }
}