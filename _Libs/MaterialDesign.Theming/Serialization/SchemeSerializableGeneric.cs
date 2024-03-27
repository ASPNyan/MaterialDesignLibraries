namespace MaterialDesign.Theming.Serialization;

/// <summary>
/// A generic, readonly <see cref="IScheme"/> &amp; <see cref="ISchemeSerializable{TSelf}"/> implementing
/// Color Scheme, but mostly has explicit implementations for <see cref="IScheme"/>. This class should only be used
/// in place of methods that only accept <see cref="ISchemeSerializable{TSelf}"/> type parameters, and has the
/// <see cref="ConvertTo{TScheme}"/> method to convert itself to any existing <see cref="IScheme"/> implementations.
/// </summary>
public readonly struct SchemeSerializableGeneric() : ISchemeSerializable<SchemeSerializableGeneric>, IScheme
{
    public required HCTA? Origin { get; init; }
    public required bool IsDarkScheme { get; init; }

    public string SerializeScheme() => /*SchemeSerializer.SerializeGeneric((IScheme)this);*/ throw new Exception();
    
    public static SchemeSerializableGeneric DeserializeScheme(string serialized)
    {
        return SchemeSerializer.DeserializeGeneric(serialized, 
            s => new SchemeSerializableGeneric
            {
                Origin = s.Origin,
                IsDarkScheme = s.IsDarkScheme
            }, false);
    }

    /// <summary>
    /// Uses the provided <paramref name="constructor"/> method to convert from a
    /// <see cref="SchemeSerializer.SchemeGeneric"/> to the provided <typeparamref name="TScheme"/>.
    /// </summary>
    public TScheme ConvertTo<TScheme>(Func<SchemeSerializer.SchemeGeneric, TScheme> constructor) where TScheme : IScheme
        => constructor(new SchemeSerializer.SchemeGeneric(Origin, IsDarkScheme));

    private readonly IScheme _scheme = new Scheme();
    
    void IScheme.SetDark()
    {
        _scheme.SetDark();
    }

    void IScheme.SetLight()
    {
        _scheme.SetLight();
    }

    event Action? IScheme.OnUpdate
    {
        add => _scheme.OnUpdate += value;
        remove => _scheme.OnUpdate -= value;
    }

    HCTA IScheme.Primary => _scheme.Primary;

    HCTA IScheme.OnPrimary => _scheme.OnPrimary;

    HCTA IScheme.PrimaryContainer => _scheme.PrimaryContainer;

    HCTA IScheme.OnPrimaryContainer => _scheme.OnPrimaryContainer;

    HCTA IScheme.Secondary => _scheme.Secondary;

    HCTA IScheme.OnSecondary => _scheme.OnSecondary;

    HCTA IScheme.SecondaryContainer => _scheme.SecondaryContainer;

    HCTA IScheme.OnSecondaryContainer => _scheme.OnSecondaryContainer;

    HCTA IScheme.Tertiary => _scheme.Tertiary;

    HCTA IScheme.OnTertiary => _scheme.OnTertiary;

    HCTA IScheme.TertiaryContainer => _scheme.TertiaryContainer;

    HCTA IScheme.OnTertiaryContainer => _scheme.OnTertiaryContainer;

    HCTA IScheme.Outline => _scheme.Outline;

    HCTA IScheme.OutlineVariant => _scheme.OutlineVariant;

    HCTA IScheme.Background => _scheme.Background;

    HCTA IScheme.OnBackground => _scheme.OnBackground;

    HCTA IScheme.Surface => _scheme.Surface;

    HCTA IScheme.OnSurface => _scheme.OnSurface;

    HCTA IScheme.SurfaceVariant => _scheme.SurfaceVariant;

    HCTA IScheme.OnSurfaceVariant => _scheme.OnSurfaceVariant;

    HCTA IScheme.SurfaceInverse => _scheme.SurfaceInverse;

    HCTA IScheme.OnSurfaceInverse => _scheme.OnSurfaceInverse;

    HCTA IScheme.SurfaceBright => _scheme.SurfaceBright;

    HCTA IScheme.SurfaceDim => _scheme.SurfaceDim;

    HCTA IScheme.SurfaceContainer => _scheme.SurfaceContainer;

    HCTA IScheme.SurfaceContainerLow => _scheme.SurfaceContainerLow;

    HCTA IScheme.SurfaceContainerLowest => _scheme.SurfaceContainerLowest;

    HCTA IScheme.SurfaceContainerHigh => _scheme.SurfaceContainerHigh;

    HCTA IScheme.SurfaceContainerHighest => _scheme.SurfaceContainerHighest;

    HCTA IScheme.PrimaryFixed => _scheme.PrimaryFixed;

    HCTA IScheme.PrimaryFixedDim => _scheme.PrimaryFixedDim;

    HCTA IScheme.OnPrimaryFixed => _scheme.OnPrimaryFixed;

    HCTA IScheme.OnPrimaryFixedVariant => _scheme.OnPrimaryFixedVariant;

    HCTA IScheme.SecondaryFixed => _scheme.SecondaryFixed;

    HCTA IScheme.SecondaryFixedDim => _scheme.SecondaryFixedDim;

    HCTA IScheme.OnSecondaryFixed => _scheme.OnSecondaryFixed;

    HCTA IScheme.OnSecondaryFixedVariant => _scheme.OnSecondaryFixedVariant;

    HCTA IScheme.TertiaryFixed => _scheme.TertiaryFixed;

    HCTA IScheme.TertiaryFixedDim => _scheme.TertiaryFixedDim;

    HCTA IScheme.OnTertiaryFixed => _scheme.OnTertiaryFixed;

    HCTA IScheme.OnTertiaryFixedVariant => _scheme.OnTertiaryFixedVariant;
}