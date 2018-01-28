A simple example of doing custom themes with MaterialDesignInXAML (MDIX) and MahApps.

There are several points worth noting in this project.
- In App.xaml the the appropriate resource dictionaries are merged in. This follows the [example](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit/blob/master/MahMaterialDragablzMashUp/App.xaml) given in the MDIX library.
- Also in App.xaml there is a list of MahApps brushes and their default correspoding colors. For this example I am only using a total of 4 colors: light, mid, dark, accent. You can certainly extend this if you need more in your application.
- The themes displayed in this application are created from three difference sources: MDIX SwatchesProvider, a custom resource dictionary, and from code.
- The MDIX PaletteHelper (currently) makes some assumptions about the names of all of the MahApps brushes. However it appears that a few more have been added. To handle this there is a MahAppPaletteHelper class that derives from the MDIX PaletteHelper that takes these extra brushes into account.
- MDIX has a couple swatches that do not provide accent colors; these are not included here. For simplicity, in this example, I have merge primary and accent colors into a single "Theme". Separating these back out into primary and accent swatches is left as an excercise to the reader.
