using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
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
            PrimayLightHue = primaryHues[lightIndex];
            PrimayMidHue = primaryHues[midIndex];
            PrimayDarkHue = primaryHues[darkIndex];
            SecondaryAccentHue = swatch.AccentExemplarHue;
        }

        public Theme(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public Hue PrimayLightHue { get; set; }
        public Hue PrimayMidHue { get; set; }
        public Hue PrimayDarkHue { get; set; }

        public Hue SecondaryAccentHue { get; set; }

        public Palette GetPalette()
        {
            if (PrimayLightHue == null) throw new InvalidOperationException($"{nameof(PrimayLightHue)} is required");
            if (PrimayMidHue == null) throw new InvalidOperationException($"{nameof(PrimayMidHue)} is required");
            if (PrimayDarkHue == null) throw new InvalidOperationException($"{nameof(PrimayDarkHue)} is required");
            if (SecondaryAccentHue == null) throw new InvalidOperationException($"{nameof(SecondaryAccentHue)} is required");
            var swatch = new Swatch("CustomSwatch", new[] { PrimayLightHue, PrimayMidHue, PrimayDarkHue }, new[] { SecondaryAccentHue });
            return new Palette(swatch, swatch, 0, 1, 2, 0);
        }
    }
}