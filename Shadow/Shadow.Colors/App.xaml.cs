using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;

namespace Shadow.Colors
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shadowInfoType = typeof(ShadowConverter).Assembly.GetType("MaterialDesignThemes.Wpf.Converters.ShadowInfo");
            var field = shadowInfoType.GetField("ShadowsDictionary", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null)
            {
                var shadowDictionary = (IDictionary<ShadowDepth, DropShadowEffect>)field.GetValue(null);
                var newColor = AsTrasparent(System.Windows.Media.Colors.Fuchsia);
                foreach (ShadowDepth depth in shadowDictionary.Keys.ToList())
                {
                    shadowDictionary[depth] = ChangeDropShadowColor(shadowDictionary[depth], newColor);
                }
            }

            Color AsTrasparent(Color color)
            {
                return Color.FromArgb(0xAA, 
                    color.R,
                    color.G,
                    color.B);
            }

            DropShadowEffect ChangeDropShadowColor(DropShadowEffect effect, Color color)
            {
                if (effect == null) return null;

                return new DropShadowEffect()
                {
                    BlurRadius = effect.BlurRadius,
                    Color = color,
                    Direction = effect.Direction,
                    Opacity = effect.Opacity,
                    RenderingBias = effect.RenderingBias,
                    ShadowDepth = effect.ShadowDepth
                };
            }
        }
    }
}
