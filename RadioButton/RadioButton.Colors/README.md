# RadioButton.Colors

RadioButton.Colors is an example project which shows how to set colors in the radio button 

## Visual

![Animated GIF of project output](Assets/RatioButtonColors.gif)

## Code documentation

### XAML

Shows how to set colors on the different parts of the RatioButton.

```xaml
<RadioButton Content="First">
    <RadioButton.Resources>
        <SolidColorBrush x:Key="MaterialDesignCheckBoxOff" Color="Red" />
        <SolidColorBrush x:Key="PrimaryHueMidBrush" Color="Lime" />
    </RadioButton.Resources>
</RadioButton>
<RadioButton Content="Second" Margin="0,10,0,0" Background="BlueViolet" Foreground="Yellow" />
```