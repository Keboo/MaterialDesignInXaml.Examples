using System;
using System.Windows.Media;

namespace CustomTheme
{
    public class ThemeColorViewModel
    {
        public ThemeColorViewModel(Theme theme)
        {
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
            SampleBrush = new SolidColorBrush
            {
                Color = theme.PrimaryMidHue.Color
            };
            Name = theme.Name;
        }

        public Brush SampleBrush { get; }

        public string Name { get; }

        public Theme Theme { get; }
    }
}