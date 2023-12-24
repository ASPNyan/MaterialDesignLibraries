using MaterialDesign.Color.Blend;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Theming;

public class Theme
{
    public bool IsDarkScheme { get; private set; }
    public Scheme CurrentScheme => IsDarkScheme ? Scheme with { IsDark = true } : Scheme with { IsDark = false };
    public Scheme Scheme { get; private set; }

    public void SetDark() => IsDarkScheme = true;
    public void SetLight() => IsDarkScheme = false;

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
    
    private int Core => IsDarkScheme ? 80 : 40;
    private int OnCore => Core + SignViaDark(60);
    private int CoreContainer => Core + SignViaDark(50);
    private int OnCoreContainer => IsDarkScheme ? 90 : 10;
    private int SignViaDark(int tone) => IsDarkScheme ? -tone : tone;

    public HCTA Error => CorePalette.ErrorPalette.GetWithTone(Core);
    public HCTA OnError => CorePalette.ErrorPalette.GetWithTone(OnCore);
    public HCTA ErrorContainer => CorePalette.ErrorPalette.GetWithTone(CoreContainer);
    public HCTA OnErrorContainer => CorePalette.ErrorPalette.GetWithTone(OnCoreContainer);

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
        TonalPalette primaryPalette = new(primary);
        TonalPalette secondaryPalette = new(secondary);
        TonalPalette tertiaryPalette = new(tertiary);
        TonalPalette neutralPalette = new(neutral);
        TonalPalette neutralVariantPalette = new(primary.H, 8);

        Scheme scheme = new(primaryPalette, secondaryPalette, tertiaryPalette,
            neutralPalette, neutralVariantPalette, true);

        Scheme = scheme;
    }

    public Theme(HCTA input)
    {
        ThemeScheme core = new(input, true);

        Scheme scheme = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, true);

        Scheme = scheme;
    }

    public void Update(HCTA? primarySource, HCTA? secondarySource, HCTA? tertiarySource = null, HCTA? neutralSource = null)
    {
        if (primarySource is null && secondarySource is null && tertiarySource is null && neutralSource is null) return;
        var (primary, secondary, tertiary, neutral, neutralVariant) = Scheme.Sources;
        Scheme scheme = new(NewOrDefault(primarySource, primary), NewOrDefault(secondarySource, secondary),
            NewOrDefault(tertiarySource, tertiary), NewOrDefault(neutralSource, neutral),
            NewOrDefault(neutralSource, neutralVariant), true);

        Scheme = scheme;
        
        OnUpdate?.Invoke();

        return;

        TonalPalette NewOrDefault(HCTA? value, TonalPalette defaultPalette) =>
            value is null ? defaultPalette : new TonalPalette(value);
    }

    public void Update(HCTA input)
    {
        ThemeScheme core = new(input, true);

        Scheme scheme = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, true);

        Scheme = scheme;
        
        OnUpdate?.Invoke();
    }

    private Dictionary<string, ColorRole> CustomColorRoles { get; } = new();
    
    public bool TryAddCustomColorRole(string id, HCTA source, bool harmonize = false) => 
        CustomColorRoles.TryAdd(id, new ColorRole(source, harmonize));

    public bool TryGetCustomColorRole(string id, out TonalPalette? palette)
    {
        bool success = CustomColorRoles.TryGetValue(id, out ColorRole role);
        palette = !success
            ? null
            : new TonalPalette(role.Harmonize ? Blend.Harmonize(Primary, role.Source) : role.Source);
        return success;
    }

    public event Action? OnUpdate;
}
