using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Theming;

public class Theme
{
    public bool IsDarkScheme { get; set; }
    public Scheme CurrentScheme => IsDarkScheme ? Schemes.Dark : Schemes.Light;
    public (Scheme Dark, Scheme Light) Schemes { get; private set; }

    public HCTA Primary => CurrentScheme.Primary;
    public HCTA OnPrimary => CurrentScheme.OnPrimary;
    public HCTA PrimaryContainer => CurrentScheme.PrimaryContainer;
    public HCTA OnPrimaryContainer => CurrentScheme.OnPrimaryContainer;

    public HCTA Secondary => CurrentScheme.Secondary;
    public HCTA OnSecondary => CurrentScheme.OnSecondary;
    public HCTA SecondaryContainer => CurrentScheme.SecondaryContainer;
    public HCTA OnSecondaryContainer => CurrentScheme.OnSecondaryContainer;

    public HCTA Tertiary => CurrentScheme.Tertiary;
    public HCTA OnTertiary => CurrentScheme.OnTertiary;
    public HCTA TertiaryContainer => CurrentScheme.TertiaryContainer;
    public HCTA OnTertiaryContainer => CurrentScheme.OnTertiaryContainer;

    public HCTA Outline => CurrentScheme.Outline;
    public HCTA OutlineVariant => CurrentScheme.OutlineVariant;

    public HCTA Background => CurrentScheme.Background;
    public HCTA OnBackground => CurrentScheme.OnBackground;
    public HCTA Surface => Background;
    public HCTA OnSurface => OnBackground;
    public HCTA SurfaceVariant => CurrentScheme.SurfaceVariant;
    public HCTA OnSurfaceVariant => CurrentScheme.OnSurfaceVariant;

    public HCTA SurfaceInverse => CurrentScheme.SurfaceInverse;
    public HCTA OnSurfaceInverse => CurrentScheme.OnSurfaceInverse;
    public HCTA SurfaceBright => CurrentScheme.SurfaceBright;
    public HCTA SurfaceDim => CurrentScheme.SurfaceDim;

    public HCTA SurfaceContainer => CurrentScheme.SurfaceContainer;
    public HCTA SurfaceContainerLow => CurrentScheme.SurfaceContainerLow;
    public HCTA SurfaceContainerLowest => CurrentScheme.SurfaceContainerLowest;
    public HCTA SurfaceContainerHigh => CurrentScheme.SurfaceContainerHigh;
    public HCTA SurfaceContainerHighest => CurrentScheme.SurfaceContainerHighest;

    public HCTA PrimaryFixed => CurrentScheme.PrimaryFixed;
    public HCTA PrimaryFixedDim => CurrentScheme.PrimaryFixedDim;
    public HCTA OnPrimaryFixed => CurrentScheme.OnPrimaryFixed;
    public HCTA OnPrimaryFixedVariant => CurrentScheme.OnPrimaryFixedVariant;
    
    public HCTA SecondaryFixed => CurrentScheme.SecondaryFixed;
    public HCTA SecondaryFixedDim => CurrentScheme.SecondaryFixedDim;
    public HCTA OnSecondaryFixed => CurrentScheme.OnSecondaryFixed;
    public HCTA OnSecondaryFixedVariant => CurrentScheme.OnSecondaryFixedVariant;
    
    public HCTA TertiaryFixed => CurrentScheme.TertiaryFixed;
    public HCTA TertiaryFixedDim => CurrentScheme.TertiaryFixedDim;
    public HCTA OnTertiaryFixed => CurrentScheme.OnTertiaryFixed;
    public HCTA OnTertiaryFixedVariant => CurrentScheme.OnTertiaryFixedVariant;
    
    public Theme(HCTA primary, HCTA secondary, HCTA tertiary, HCTA neutral)
    {
        CorePalette primaryPalette = new CorePalette(primary);
        CorePalette secondaryPalette = new CorePalette(secondary);
        CorePalette tertiaryPalette = new CorePalette(tertiary);
        CorePalette neutralPalette = new CorePalette(neutral);

        Scheme dark = new(primaryPalette.Primary, secondaryPalette.Primary, tertiaryPalette.Primary,
            neutralPalette.Neutral, neutralPalette.NeutralVariant, true);
        Scheme light = new(primaryPalette.Primary, secondaryPalette.Primary, tertiaryPalette.Primary,
            neutralPalette.Neutral, neutralPalette.NeutralVariant, false);

        Schemes = (dark, light);
    }

    public Theme(HCTA input)
    {
        CorePalette core = new(input);

        Scheme dark = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, true);
        Scheme light = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, false);

        Schemes = (dark, light);
    }

    public void Update(HCTA? primary, HCTA? secondary, HCTA? tertiary = null, HCTA? neutral = null)
    {
        if (primary is null && secondary is null && tertiary is null && neutral is null) return;
        var (primaryDark, secondaryDark, tertiaryDark, neutralDark, neutralVariantDark) = Schemes.Dark.Sources;
        Scheme dark = new(NewOrDefault(primary, primaryDark), NewOrDefault(secondary, secondaryDark),
            NewOrDefault(tertiary, tertiaryDark), NewOrDefault(neutral, neutralDark),
            NewOrDefault(neutral, neutralVariantDark), true);
        
        var (primaryLight, secondaryLight, tertiaryLight, neutralLight, neutralVariantLight) = Schemes.Light.Sources;
        Scheme light = new(NewOrDefault(primary, primaryLight), NewOrDefault(secondary, secondaryLight),
            NewOrDefault(tertiary, tertiaryLight), NewOrDefault(neutral, neutralLight),
            NewOrDefault(neutral, neutralVariantLight), true);

        Schemes = (dark, light);
        
        OnUpdate?.Invoke();

        return;

        TonalPalette NewOrDefault(HCTA? value, TonalPalette defaultPalette) =>
            value is null ? defaultPalette : new TonalPalette(value);
    }

    public void Update(HCTA input)
    {
        CorePalette core = new(input);

        Scheme dark = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, true);
        Scheme light = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, false);

        Schemes = (dark, light);
        
        OnUpdate?.Invoke();
    }

    public event Action? OnUpdate;
}
