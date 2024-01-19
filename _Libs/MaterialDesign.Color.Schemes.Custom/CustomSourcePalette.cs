using MaterialDesign.Color.Extensions;

namespace MaterialDesign.Color.Schemes.Custom;

public readonly struct CustomSourcePalette(TonalPalette source, double coreDarkTone, double coreLightTone, 
    double onColorContrast, double coreContainerContrast, Func<HCTA, HCTA> colorDiffMethod)
{
    public HCTA Core(bool isDark) => colorDiffMethod(source.GetWithTone(isDark ? coreDarkTone : coreLightTone));
    public HCTA OnCore(bool isDark) => colorDiffMethod(Core(isDark).ContrastTo(onColorContrast, !isDark));
    public HCTA Container(bool isDark) => colorDiffMethod(Core(isDark).ContrastTo(coreContainerContrast, !isDark));
    public HCTA OnContainer(bool isDark) => colorDiffMethod(Container(isDark).ContrastTo(onColorContrast, isDark));
}