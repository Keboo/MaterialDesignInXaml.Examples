using System;
using System.Windows.Media;
using MaterialDesignColors;

namespace CustomTheme
{
    public class ThemeColorViewModel
    {
        private readonly Swatch _Swatch;

        public ThemeColorViewModel(Swatch swatch)
        {
            _Swatch = swatch ?? throw new ArgumentNullException(nameof(swatch));
            SampleBrush = new SolidColorBrush
            {
                Color = swatch.ExemplarHue.Color
            };
            Name = swatch.Name;
        }

        public Brush SampleBrush { get; }

        public string Name { get; }
    }
}