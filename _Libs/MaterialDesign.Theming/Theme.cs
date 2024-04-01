using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MaterialDesign.Color.Blend;
using MaterialDesign.Color.Palettes;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Serialization;

namespace MaterialDesign.Theming;

/// <summary>
/// A Material Theme, containing getters for all colors in the active <see cref="Scheme"/> as well as methods to update
/// the scheme, and support for custom color roles (see https://m3.material.io/styles/color/advanced/define-new-colors)
/// </summary>
public class Theme : IThemeSource, IScheme, ISchemeSerializable<Theme>, IEquatable<Theme>
{
    /// <summary>
    /// The color that created the theme, null when theme is created with multiple colours.
    /// </summary>
    public HCTA? Origin { get; private set; }
    public bool IsDarkScheme { get; private set; }
    public Scheme CurrentScheme => IsDarkScheme ? Scheme with { IsDark = true } : Scheme with { IsDark = false };
    public Scheme Scheme { get; private set; }
    
    public TextColorStyle TextColorStyle { get; private set; }

    /// <summary>
    /// Sets a theme to dark mode. Does nothing if this is already the case.
    /// </summary>
    public void SetDark()
    {
        if (IsDarkScheme) return;
        IsDarkScheme = true;
        OnUpdate?.Invoke();
    }

    /// <summary>
    /// Sets a theme to light mode. Does nothing if this is already the case.
    /// </summary>
    public void SetLight()
    {
        if (!IsDarkScheme) return;
        IsDarkScheme = false;
        OnUpdate?.Invoke();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void SetColoredText()
    {
        if (TextColorStyle is TextColorStyle.Colored) return;
        TextColorStyle = TextColorStyle.Colored;
        OnUpdate?.Invoke();
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void SetBlackAndWhiteText()
    {
        if (TextColorStyle is TextColorStyle.BlackAndWhite) return;
        TextColorStyle = TextColorStyle.BlackAndWhite;
        OnUpdate?.Invoke();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void SetGreyText()
    {
        if (TextColorStyle is TextColorStyle.GreyAndGrey) return;
        TextColorStyle = TextColorStyle.GreyAndGrey;
        OnUpdate?.Invoke();
    }

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
    
    /// <summary>
    /// Creates a theme based on four individual colors. <see cref="Origin"/> is null when this is used, as it only
    /// supplies one color.
    /// </summary>
    /// <param name="primary">The primary color, also used for neutral variant colors.</param>
    /// <param name="secondary">The secondary color</param>
    /// <param name="tertiary">The tertiary color</param>
    /// <param name="neutral">The neutral color</param>
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

    /// <summary>
    /// Creates a theme based on a single color, which is used as the primary color.
    /// </summary>
    /// <param name="input">The input color, which is used to generate the remaining colors with <see cref="ThemeScheme"/>.</param>
    public Theme(HCTA input)
    {
        Origin = input;
        
        ThemeScheme core = new(input, true);

        Scheme scheme = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, true);

        Scheme = scheme;
    }

    /// <summary>
    /// Updates the current theme based on four individual colors. <see cref="Origin"/> is null when this is used, as it only
    /// supplies one color. Null sources are treated as unchanged, and will just get the current relevant color.
    /// <see cref="Origin"/> is set to null when this is used, as it can only contain a single color.
    /// </summary>
    /// <param name="primarySource">The primary color, also used for neutral variant colors.</param>
    /// <param name="secondarySource">The secondary color</param>
    /// <param name="tertiarySource">The tertiary color</param>
    /// <param name="neutralSource">The neutral color</param>
    public void Update(HCTA? primarySource, HCTA? secondarySource, HCTA? tertiarySource = null, HCTA? neutralSource = null)
    {
        if (primarySource is null && secondarySource is null && tertiarySource is null && neutralSource is null) return;
        Origin = null;
        
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

    /// <summary>
    /// Updates the entire current theme by basing it on a single color, which is used as the primary color.
    /// </summary>
    /// <param name="input">The input color, which is used to generate the remaining colors with <see cref="ThemeScheme"/>.</param>
    public void Update(HCTA input)
    {
        Origin = input;
        
        ThemeScheme core = new(input, true);

        Scheme scheme = new(core.Primary, core.Secondary, core.Tertiary, core.Neutral, core.NeutralVariant, true);

        Scheme = scheme;
        
        OnUpdate?.Invoke();
    }

    private Dictionary<string, ColorRole> CustomColorRoles { get; } = new();

    /// <summary>
    /// Tries to add a custom color role to the theme. See https://m3.material.io/styles/color/advanced/define-new-colors
    /// </summary>
    /// <param name="id">The Id of the custom role, used to store the role in a Dictionary.</param>
    /// <param name="source">The color role source color.</param>
    /// <param name="harmonize">Whether the color should be harmonized to match the primary hue or not.</param>
    /// <returns>A bool indicating the success of adding the value to the custom roles dictionary.</returns>
    public bool TryAddCustomColorRole(string id, HCTA source, bool harmonize = false) =>
        CustomColorRoles.TryAdd(id, new ColorRole(source, harmonize));

    /// <summary>
    /// Tries to get a custom color role from the current theme.
    /// </summary>
    /// <param name="id">The Id of the role in the dictionary.</param>
    /// <param name="palette">The palette stored.</param>
    /// <returns>A bool indicating the success of getting the value.</returns>
    public bool TryGetCustomColorRole(string id, [NotNullWhen(true)] out TonalPalette? palette)
    {
        bool success = CustomColorRoles.TryGetValue(id, out ColorRole role);
        palette = !success
            ? null
            : new TonalPalette(role.Harmonize ? Blend.Harmonize(Primary, role.Source) : role.Source);
        return success;
    }

    public event Action? OnUpdate;
    
    #region IThemeSource

    Theme IThemeSource.UseActualTheme => this;

    #endregion

    #region ISchemeSerializable

    public string SerializeScheme() => 
        JsonSerializer.Serialize(new SerializableTheme(Origin, Scheme, IsDarkScheme, CustomColorRoles));

    public static Theme DeserializeScheme(string serialized)
    {
        SerializableTheme? temp = JsonSerializer.Deserialize<SerializableTheme>(serialized);
        
        if (temp is null) throw new JsonException("Unable to deserialize provided Theme.");

        Theme theme;
        if (temp.Origin is null)
        {
            var (primary, secondary, tertiary, neutral, _) = temp.Scheme.Sources;
            theme = new Theme(primary.KeyColor, secondary.KeyColor, tertiary.KeyColor, neutral.KeyColor);
        }
        else theme = new Theme(temp.Origin);

        foreach (KeyValuePair<string, ColorRole> colorRole in temp.CustomColorRoles)
            theme.TryAddCustomColorRole(colorRole.Key, colorRole.Value.Source, colorRole.Value.Harmonize);

        return theme;
    }

    private record SerializableTheme(HCTA? Origin, Scheme Scheme, bool IsDarkScheme, 
        Dictionary<string, ColorRole> CustomColorRoles);

    #endregion

    #region Equals

    public bool Equals(Theme? other) => other is not null && Origin == other.Origin && Scheme == other.Scheme;
    public override bool Equals(object? other) => other is Theme theme && Equals(theme);

    private readonly Guid _hash = Guid.NewGuid();
    
    public override int GetHashCode() => _hash.GetHashCode();

    public static bool operator ==(Theme? left, Theme? right) => Equals(left, right);

    public static bool operator !=(Theme? left, Theme? right) => !Equals(left, right);

    #endregion
}
