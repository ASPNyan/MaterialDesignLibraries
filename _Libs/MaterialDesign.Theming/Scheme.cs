using System.Text.Json;
using MaterialDesign.Color.Palettes;
using MaterialDesign.Theming.Serialization;

namespace MaterialDesign.Theming;

/// <summary>
/// A Material Design color scheme, providing all the different types of color roles provided by default, alongside
/// fixed, dimmed, and variant colors, all based on whether the scheme is light or dark.
/// </summary>
public readonly struct Scheme(
    TonalPalette primary,
    TonalPalette secondary,
    TonalPalette tertiary,
    TonalPalette neutral,
    TonalPalette neutralVariant,
    bool isDark) : IScheme, IEquatable<Scheme>, ISchemeSerializable<Scheme>
{
    public HCTA? Origin => null;
    public bool IsDark { get; init; } = isDark;

    public (TonalPalette Primary, TonalPalette Secondary, TonalPalette Tertiary, TonalPalette Neutral,
        TonalPalette NeutralVariant) Sources { get; } = (primary, secondary, tertiary, neutral, neutralVariant);
    
    private int Core => IsDark ? 80 : 40;
    private int OnCore => Core + SignViaDark(60);
    private int CoreContainer => Core + SignViaDark(50);
    private int OnCoreContainer => IsDark ? 90 : 10;

    bool IScheme.IsDarkScheme => IsDark;

    void IScheme.SetDark() => 
        throw new InvalidOperationException("Swapping between dark and light mode is not supported on Theming.Scheme");

    void IScheme.SetLight() => 
        throw new InvalidOperationException("Swapping between dark and light mode is not supported on Theming.Scheme");
    
    event Action? IScheme.OnUpdate
    {
        add { }
        remove { }
    }

    public HCTA Primary => primary.GetWithTone(Core);
    public HCTA OnPrimary => primary.GetWithTone(OnCore);
    public HCTA PrimaryContainer => primary.GetWithTone(CoreContainer);
    public HCTA OnPrimaryContainer => primary.GetWithTone(OnCoreContainer);
    
    public HCTA Secondary => secondary.GetWithTone(Core);
    public HCTA OnSecondary => secondary.GetWithTone(OnCore);
    public HCTA SecondaryContainer => secondary.GetWithTone(CoreContainer);
    public HCTA OnSecondaryContainer => secondary.GetWithTone(OnCoreContainer);
    
    public HCTA Tertiary => tertiary.GetWithTone(Core);
    public HCTA OnTertiary => tertiary.GetWithTone(OnCore);
    public HCTA TertiaryContainer => tertiary.GetWithTone(CoreContainer);
    public HCTA OnTertiaryContainer => tertiary.GetWithTone(OnCoreContainer);

    public HCTA Outline => neutralVariant.GetWithTone(IsDark ? 60 : 50);
    public HCTA OutlineVariant => neutralVariant.GetWithTone(IsDark ? 80 : 30);
    
    public HCTA Background => neutral.GetWithTone(IsDark ? 6 : 98);
    public HCTA OnBackground => neutral.GetWithTone(IsDark ? 90 : 10);
    public HCTA Surface => Background;
    public HCTA OnSurface => OnBackground;
    public HCTA SurfaceVariant => neutralVariant.GetWithTone(IsDark ? 30 : 90);
    public HCTA OnSurfaceVariant => neutralVariant.GetWithTone(IsDark ? 80 : 30);
    
    public HCTA SurfaceInverse => neutral.GetWithTone(IsDark ? 90 : 20);
    public HCTA OnSurfaceInverse => neutral.GetWithTone(IsDark ? 20 : 95);
    public HCTA SurfaceBright => neutral.GetWithTone(IsDark ? 24 : 98);
    public HCTA SurfaceDim => neutral.GetWithTone(IsDark ? 6 : 87);
    
    public HCTA SurfaceContainer => neutral.GetWithTone(IsDark ? 12 : 94);
    public HCTA SurfaceContainerLow => neutral.GetWithTone(IsDark ? 10 : 96);
    public HCTA SurfaceContainerLowest => neutral.GetWithTone(IsDark ? 4 : 100);
    public HCTA SurfaceContainerHigh => neutral.GetWithTone(IsDark ? 17 : 92);
    public HCTA SurfaceContainerHighest => neutral.GetWithTone(IsDark ? 22 : 90);

    public HCTA PrimaryFixed => primary.GetWithTone(90);
    public HCTA PrimaryFixedDim => primary.GetWithTone(80);
    public HCTA OnPrimaryFixed => primary.GetWithTone(10);
    public HCTA OnPrimaryFixedVariant => primary.GetWithTone(30);
    
    public HCTA SecondaryFixed => secondary.GetWithTone(90);
    public HCTA SecondaryFixedDim => secondary.GetWithTone(80);
    public HCTA OnSecondaryFixed => secondary.GetWithTone(10);
    public HCTA OnSecondaryFixedVariant => secondary.GetWithTone(30);
    
    public HCTA TertiaryFixed => tertiary.GetWithTone(90);
    public HCTA TertiaryFixedDim => tertiary.GetWithTone(80);
    public HCTA OnTertiaryFixed => tertiary.GetWithTone(10);
    public HCTA OnTertiaryFixedVariant => tertiary.GetWithTone(30);
    
    private int SignViaDark(int tone) => IsDark ? -tone : tone;

    public bool Equals(Scheme other) =>
        IsDark == other.IsDark
        && Sources.Primary.KeyColor == other.Sources.Primary.KeyColor
        && Sources.Secondary.KeyColor == other.Sources.Secondary.KeyColor
        && Sources.Tertiary.KeyColor == other.Sources.Tertiary.KeyColor
        && Sources.Neutral.KeyColor == other.Sources.Neutral.KeyColor
        && Sources.NeutralVariant.KeyColor == other.Sources.NeutralVariant.KeyColor;

    public override int GetHashCode() => HashCode.Combine(IsDark, Sources);

    public override bool Equals(object? obj) => obj is Scheme other && Equals(other);

    public static bool operator ==(Scheme left, Scheme right) => left.Equals(right);

    public static bool operator !=(Scheme left, Scheme right) => !left.Equals(right);

    #region ISchemeSerializable

    public string SerializeScheme() => 
        JsonSerializer.Serialize(new SerializableScheme(primary, secondary, tertiary, neutral, neutralVariant, IsDark));

    public static Scheme DeserializeScheme(string serialized)
    {
        SerializableScheme? scheme = JsonSerializer.Deserialize<SerializableScheme>(serialized);
        if (scheme is null) throw new JsonException("Unable to deserialize provided Scheme.");

        return new Scheme(scheme.Primary, scheme.Secondary, scheme.Tertiary, scheme.Neutral, scheme.NeutralVariant, 
            scheme.IsDark);
    }

    private record SerializableScheme(
        TonalPalette Primary,
        TonalPalette Secondary,
        TonalPalette Tertiary,
        TonalPalette Neutral,
        TonalPalette NeutralVariant,
        bool IsDark);

    #endregion
}