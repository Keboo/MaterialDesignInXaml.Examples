# Menu.HorizontalDisplay

Menu.HorizontalDisplay is an example project which shows how to setup a horizontal menu. 

## Visual

![Animated GIF of project output](Assets/MenuHorizontalDisplay.gif)

## Code documentation

### XAML

Shows how `DrawerHost` with a `DockPanel` to hold the `Menu` are used to create the Menu.HorizontalDisplay.

```xaml
<materialDesign:DrawerHost IsLeftDrawerOpen="{Binding ElementName=MenuToggleButton, Path=IsChecked}">
    <materialDesign:DrawerHost.LeftDrawerContent>
        <DockPanel MinWidth="212">
            <ToggleButton Style="{StaticResource MaterialDesignHamburgerToggleButton}" 
                            DockPanel.Dock="Top"
                            HorizontalAlignment="Right" Margin="16"
                            IsChecked="{Binding ElementName=MenuToggleButton, Path=IsChecked, Mode=TwoWay}" />
            <Menu>
                <Menu.Resources>
                    <Style TargetType="MenuItem" BasedOn="{StaticResource {x:Type MenuItem}}">
                        <Setter Property="utilities:TreeHelpers.Modifiers">
                            <Setter.Value>
                                <utilities:ModifierCollection>
                                    <utilities:Modifier Property="{x:Static Popup.PlacementProperty}" 
                                                        Value="{x:Static PlacementMode.Right}" 
                                                        TemplatePartName="PART_Popup" />
                                </utilities:ModifierCollection>
                            </Setter.Value>
                        </Setter>
                    </Style>
                </Menu.Resources>
                <Menu.ItemsPanel>
                    <ItemsPanelTemplate>
                        <VirtualizingStackPanel />
                    </ItemsPanelTemplate>
                </Menu.ItemsPanel>
                <MenuItem Header="Item 1">
                    <MenuItem Header="Sub Item 1" />
                    <MenuItem Header="Sub Item 2" />
                    <MenuItem Header="Sub Item 3" />
                </MenuItem>
                <MenuItem Header="Item 2">
                    ...
                </MenuItem>
                <MenuItem Header="Item 3">
                    ...
                </MenuItem>
            </Menu>
        </DockPanel>
    </materialDesign:DrawerHost.LeftDrawerContent>
        
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition />
        </Grid.RowDefinitions>
            
        <materialDesign:ColorZone Padding="16" materialDesign:ShadowAssist.ShadowDepth="Depth2"
                                    Mode="PrimaryMid" DockPanel.Dock="Top">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto" />
                    <ColumnDefinition />
                </Grid.ColumnDefinitions>
                <ToggleButton Style="{StaticResource MaterialDesignHamburgerToggleButton}" IsChecked="False"
                                x:Name="MenuToggleButton" Grid.Column="0"/>
            </Grid>
        </materialDesign:ColorZone>
    </Grid>
</materialDesign:DrawerHost>
```