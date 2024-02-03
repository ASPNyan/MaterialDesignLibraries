using MaterialDesign.Color.Extensions;
using _Contrast = MaterialDesign.Color.Contrast.Contrast;

namespace MaterialDesign.Color.Schemes.Custom;

public readonly struct CustomSourcePalette(TonalPalette source, double coreDarkTone, double coreLightTone, 
    double onColorContrast, double coreContainerContrast, Func<HCTA, HCTA> colorDiffMethod)
{
    public HCTA Core(bool isDark) => colorDiffMethod(source.GetWithTone(isDark ? coreDarkTone : coreLightTone));
    
    public HCTA OnCore(bool isDark)
    {
        double upperMin = _Contrast.LighterViaRatio(0, onColorContrast);
        double lowerMax = _Contrast.DarkerViaRatio(100, onColorContrast);
        HCTA core = Core(isDark);
        if (core.T > lowerMax && core.T < upperMin)
            throw new Exception($"A core tone of {core.T:N2} is not valid with an onColorContrast " +
                                $"of {onColorContrast:N2}. Please modify your values accordingly");
        return colorDiffMethod(core.ContrastTo(onColorContrast, !isDark));
    }

    public HCTA Container(bool isDark)
    {
        double upperMin = _Contrast.LighterViaRatio(0, coreContainerContrast);
        double lowerMax = _Contrast.DarkerViaRatio(100, coreContainerContrast);
        HCTA core = Core(isDark);
        if (core.T > lowerMax && core.T < upperMin)
            throw new Exception($"A core tone of {core.T:N2} is not valid with a core container contrast " +
                                $"of {coreContainerContrast:N2}. Please modify your values accordingly");
        return colorDiffMethod(core.ContrastTo(coreContainerContrast, !isDark));
    }

    public HCTA OnContainer(bool isDark)
    {
        double upperMin = _Contrast.LighterViaRatio(0, onColorContrast);
        double lowerMax = _Contrast.DarkerViaRatio(100, onColorContrast);
        HCTA container = Container(isDark);
        if (container.T > lowerMax && container.T < upperMin)
            throw new Exception($"An onColorContrast of {onColorContrast:N2} is not valid with a core container " +
                                $"contrast of {coreContainerContrast:N2}. Please modify your values accordingly");
        return colorDiffMethod(container.ContrastTo(onColorContrast, isDark));
    }
}