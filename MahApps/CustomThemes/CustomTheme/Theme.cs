using System;
using System.Collections.Generic;
using System.Linq;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace CustomTheme
{
    public class Theme
    {
        public Theme(Swatch swatch)
        {
            Name = swatch.Name;
            List<Hue> primaryHues = swatch.PrimaryHues.ToList();
            //Making some assumptions.....
            int lightIndex = Math.Min(primaryHues.Count - 1, 1);
            int midIndex = Math.Min(primaryHues.Count - 1, 5);
            int darkIndex = Math.Min(primaryHues.Count - 1, 7);
            PrimaryLightHue = primaryHues[lightIndex];
            PrimaryMidHue = primaryHues[midIndex];
            PrimaryDarkHue = primaryHues[darkIndex];
            SecondaryAccentHue = swatch.AccentExemplarHue;
        }

        public Theme(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Hue PrimaryLightHue { get; set; }
        public Hue PrimaryMidHue { get; set; }
        public Hue PrimaryDarkHue { get; set; }

        public Hue SecondaryAccentHue { get; set; }

        public Palette GetPalette()
        {
            if (PrimaryLightHue == null) throw new InvalidOperationException($"{nameof(PrimaryLightHue)} is required");
            if (PrimaryMidHue == null) throw new InvalidOperationException($"{nameof(PrimaryMidHue)} is required");
            if (PrimaryDarkHue == null) throw new InvalidOperationException($"{nameof(PrimaryDarkHue)} is required");
            if (SecondaryAccentHue == null) throw new InvalidOperationException($"{nameof(SecondaryAccentHue)} is required");
            var swatch = new Swatch("CustomSwatch", new[] { PrimaryLightHue, PrimaryMidHue, PrimaryDarkHue }, new[] { SecondaryAccentHue });
            return new Palette(swatch, swatch, 0, 1, 2, 0);
        }
    }
}