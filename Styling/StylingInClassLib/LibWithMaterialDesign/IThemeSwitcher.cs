using System.Windows.Media;

namespace LibWithMaterialDesign;

// Because we can't use PaletteHelper we create our own abstraction.
// The implementation of this abstraction will need direct access to the theme ResourceDictionary.
public interface IThemeSwitcher
{
    void ChangePrimaryColor(Color primaryColor);
}
