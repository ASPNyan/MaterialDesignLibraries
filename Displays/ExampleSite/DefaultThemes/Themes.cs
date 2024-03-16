using MaterialDesign.Color.Colorspaces;

namespace ExampleSite.DefaultThemes;

public record Oceanic() : DefaultThemeBase(new HCTA(228.43, 11.092, 19.959), nameof(Oceanic));

public record Moonlight() : DefaultThemeBase(new HCTA(280.53, 15.886, 14.78), nameof(Moonlight));

public record Volcano() : DefaultThemeBase(new HCTA(27.238, 32.431, 2.774), nameof(Volcano));

public record Plasma() : DefaultThemeBase(new HCTA(299.4, 44.331, 69.505), nameof(Plasma));