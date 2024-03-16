using System.ComponentModel;

namespace ExampleSite.DefaultThemes;

public enum ThemeSelectionOptions
{
    Oceanic,
    Moonlight,
    Volcano,
    Plasma,
    Custom,
    [Description("Custom (Advanced)")]
    CustomAdvanced
}