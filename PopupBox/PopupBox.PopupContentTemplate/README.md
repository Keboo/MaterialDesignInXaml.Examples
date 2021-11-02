# PopupBoxPopupContentTemplate

PopupBoxPopupContentTemplate is an example project which shows how to `PopupBox` with a `DataTemplate`. 

## Visual

![Animated GIF of project output](Assets/PopupBoxPopupContentTemplate.gif)
> Note: This GIF has compression artifacts

## Code documentation

### XAML

The when clicked the `PopupBox` appears to displays the `DataTemplate` with the data bound in the `PopupContent`

```xaml
<Window.Resources>
    <DataTemplate x:Key="PopupDataTemplate" DataType="{x:Type local:PopupViewModel}">
        <TextBlock Text="{Binding Name, StringFormat='Hello {0}'}" Margin="10" />
    </DataTemplate>
</Window.Resources>
<Grid>
    <materialDesign:PopupBox HorizontalAlignment="Center" VerticalAlignment="Center" PlacementMode="BottomAndAlignCentres" 
                             PopupContentTemplate="{StaticResource PopupDataTemplate}" PopupContent="{Binding PopupViewModel}" />
</Grid>
```