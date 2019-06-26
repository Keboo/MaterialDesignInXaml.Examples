## Why

This is a simple project that provides a XAML friendly solution for modifying control templates at run-time.

When working with theming libraries not all properties can (or should) be exposed, however it is often preferable to simply modify a few properties on an existing template rather than to re-template a control.
This method can conceptually thought of as XAML reflection. It is also brittle and updates to a control's template cause changes made with this to fail.


## Example Usage:
Applying it directly to an element. This changes the background property of the "HeaderSite" element that is declared inside of the Expander's control template.

```XAML
<Expander Header="Header" Width="300">
    <utilities:TreeHelpers.Modifiers>
        <utilities:ModifierCollection>
            <utilities:Modifier TemplatePartName="HeaderSite" Property="{x:Static Control.BackgroundProperty}">
                <utilities:Modifier.Value>
                    <SolidColorBrush Color="LightBlue" />
                </utilities:Modifier.Value>
            </utilities:Modifier>
        </utilities:ModifierCollection>
    </utilities:TreeHelpers.Modifiers>
    ...
</Expander>
```

Applied with a Style
This changes the background property of the "SelectedBorder" element that is declared inside of the ListBoxItem's control template.
```XAML
<Style TargetType="ListBoxItem" BasedOn="{StaticResource {x:Type ListBoxItem}}">
    <Setter Property="utilities:TreeHelpers.Modifiers">
        <Setter.Value>
            <utilities:ModifierCollection>
                <utilities:Modifier TemplatePartName="SelectedBorder" Property="{x:Static Control.BackgroundProperty}">
                    <utilities:Modifier.Value>
                        <SolidColorBrush Color="Transparent" />
                    </utilities:Modifier.Value>
                </utilities:Modifier>
            </utilities:ModifierCollection>
        </Setter.Value>
    </Setter>
</Style>
```
