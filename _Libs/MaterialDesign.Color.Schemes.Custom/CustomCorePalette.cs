namespace MaterialDesign.Color.Schemes.Custom;

public readonly struct CustomCorePalette
{
    public bool BlackAndWhiteText { private get; init; }

    public HCTA GetText(Func<HCTA> colorMethod)
    {
        HCTA color = colorMethod();
        
        if (BlackAndWhiteText)
        {
            if (color.T <= 50) return new HCTA(0, 0, 0);
            return new HCTA(0, 0, 100);
        }

        return color;
    }
    
    public required Func<HCTA> Primary { get; init; }
    public required Func<HCTA> OnPrimary { get; init; }
    public required Func<HCTA> PrimaryContainer { get; init; }
    public required Func<HCTA> OnPrimaryContainer { get; init; }
    
    public required Func<HCTA> Secondary { get; init; }
    public required Func<HCTA> OnSecondary { get; init; }
    public required Func<HCTA> SecondaryContainer { get; init; }
    public required Func<HCTA> OnSecondaryContainer { get; init; }
    
    public required Func<HCTA> Tertiary { get; init; }
    public required Func<HCTA> OnTertiary { get; init; }
    public required Func<HCTA> TertiaryContainer { get; init; }
    public required Func<HCTA> OnTertiaryContainer { get; init; }

    public required Func<HCTA> Outline { get; init; }
    public required Func<HCTA> OutlineVariant { get; init; }
    
    public required Func<HCTA> Background { get; init; }
    public required Func<HCTA> OnBackground { get; init; }
    public required Func<HCTA> SurfaceVariant { get; init; }
    public required Func<HCTA> OnSurfaceVariant { get; init; }
    
    public required Func<HCTA> SurfaceInverse { get; init; }
    public required Func<HCTA> OnSurfaceInverse { get; init; }
    public required Func<HCTA> SurfaceBright { get; init; }
    public required Func<HCTA> SurfaceDim { get; init; }
    
    public required Func<HCTA> SurfaceContainer { get; init; }
    public required Func<HCTA> SurfaceContainerLow { get; init; }
    public required Func<HCTA> SurfaceContainerLowest { get; init; }
    public required Func<HCTA> SurfaceContainerHigh { get; init; }
    public required Func<HCTA> SurfaceContainerHighest { get; init; }

    private const int FixedTone = 90;
    private const int FixedDimTone = 80;
    private const int OnFixedTone = 10;
    private const int OnFixedBrightTone = 30;

    private static HCTA FixedColor(HCTA color, int tone) => new HCTA(color.H, color.C, tone);
    
    public HCTA PrimaryFixed() => FixedColor(Primary(), FixedTone);
    public HCTA PrimaryFixedDim() => FixedColor(Primary(), FixedDimTone);
    public HCTA OnPrimaryFixed() => FixedColor(Primary(), OnFixedTone);
    public HCTA OnPrimaryFixedBright() => FixedColor(Primary(), OnFixedBrightTone);
    
    public HCTA SecondaryFixed() => FixedColor(Secondary(), FixedTone);
    public HCTA SecondaryFixedDim() => FixedColor(Secondary(), FixedDimTone);
    public HCTA OnSecondaryFixed() => FixedColor(Secondary(), OnFixedTone);
    public HCTA OnSecondaryFixedBright() => FixedColor(Secondary(), OnFixedBrightTone);
    
    public HCTA TertiaryFixed() => FixedColor(Tertiary(), FixedTone);
    public HCTA TertiaryFixedDim() => FixedColor(Tertiary(), FixedDimTone);
    public HCTA OnTertiaryFixed() => FixedColor(Tertiary(), OnFixedTone);
    public HCTA OnTertiaryFixedBright() => FixedColor(Tertiary(), OnFixedBrightTone);
}