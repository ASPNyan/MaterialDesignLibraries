using MaterialDesign.Color.Extensions;

namespace MaterialDesign.Color.Schemes.Custom;

public readonly struct CustomSourcePalette(TonalPalette source, double coreDarkTone, double coreLightTone, 
    double onColorContrast, double coreContainerContrast)
{
    public HCTA Core(bool isDark) => source.GetWithTone(isDark ? coreDarkTone : coreLightTone);
    public HCTA OnCore(bool isDark) => Core(isDark).ContrastTo(onColorContrast, !isDark);
    public HCTA Container(bool isDark) => Core(isDark).ContrastTo(coreContainerContrast, !isDark);
    public HCTA OnContainer(bool isDark) => Container(isDark).ContrastTo(onColorContrast, isDark);
}