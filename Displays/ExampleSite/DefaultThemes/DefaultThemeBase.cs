using System.Diagnostics.CodeAnalysis;
using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Theming;

namespace ExampleSite.DefaultThemes;

public abstract record DefaultThemeBase(HCTA Source, [ConstantExpected] string ThemeName) : IScheme 
{
    public HCTA Source { get; } = Source;
    public string ThemeName { get; } = ThemeName;

    private Theme SchemeImplementation { get; } = new(Source);

    public void SetDark()
    {
        SchemeImplementation.SetDark();
    }

    public void SetLight()
    {
        SchemeImplementation.SetLight();
    }

    public HCTA? Origin => SchemeImplementation.Origin;

    public bool IsDarkScheme => SchemeImplementation.IsDarkScheme;

    public HCTA Primary => SchemeImplementation.Primary;

    public HCTA OnPrimary => SchemeImplementation.OnPrimary;

    public HCTA PrimaryContainer => SchemeImplementation.PrimaryContainer;

    public HCTA OnPrimaryContainer => SchemeImplementation.OnPrimaryContainer;

    public HCTA Secondary => SchemeImplementation.Secondary;

    public HCTA OnSecondary => SchemeImplementation.OnSecondary;

    public HCTA SecondaryContainer => SchemeImplementation.SecondaryContainer;

    public HCTA OnSecondaryContainer => SchemeImplementation.OnSecondaryContainer;

    public HCTA Tertiary => SchemeImplementation.Tertiary;

    public HCTA OnTertiary => SchemeImplementation.OnTertiary;

    public HCTA TertiaryContainer => SchemeImplementation.TertiaryContainer;

    public HCTA OnTertiaryContainer => SchemeImplementation.OnTertiaryContainer;

    public HCTA Outline => SchemeImplementation.Outline;

    public HCTA OutlineVariant => SchemeImplementation.OutlineVariant;

    public HCTA Background => SchemeImplementation.Background;

    public HCTA OnBackground => SchemeImplementation.OnBackground;

    public HCTA Surface => SchemeImplementation.Surface;

    public HCTA OnSurface => SchemeImplementation.OnSurface;

    public HCTA SurfaceVariant => SchemeImplementation.SurfaceVariant;

    public HCTA OnSurfaceVariant => SchemeImplementation.OnSurfaceVariant;

    public HCTA SurfaceInverse => SchemeImplementation.SurfaceInverse;

    public HCTA OnSurfaceInverse => SchemeImplementation.OnSurfaceInverse;

    public HCTA SurfaceBright => SchemeImplementation.SurfaceBright;

    public HCTA SurfaceDim => SchemeImplementation.SurfaceDim;

    public HCTA SurfaceContainer => SchemeImplementation.SurfaceContainer;

    public HCTA SurfaceContainerLow => SchemeImplementation.SurfaceContainerLow;

    public HCTA SurfaceContainerLowest => SchemeImplementation.SurfaceContainerLowest;

    public HCTA SurfaceContainerHigh => SchemeImplementation.SurfaceContainerHigh;

    public HCTA SurfaceContainerHighest => SchemeImplementation.SurfaceContainerHighest;

    public HCTA PrimaryFixed => SchemeImplementation.PrimaryFixed;

    public HCTA PrimaryFixedDim => SchemeImplementation.PrimaryFixedDim;

    public HCTA OnPrimaryFixed => SchemeImplementation.OnPrimaryFixed;

    public HCTA OnPrimaryFixedVariant => SchemeImplementation.OnPrimaryFixedVariant;

    public HCTA SecondaryFixed => SchemeImplementation.SecondaryFixed;

    public HCTA SecondaryFixedDim => SchemeImplementation.SecondaryFixedDim;

    public HCTA OnSecondaryFixed => SchemeImplementation.OnSecondaryFixed;

    public HCTA OnSecondaryFixedVariant => SchemeImplementation.OnSecondaryFixedVariant;

    public HCTA TertiaryFixed => SchemeImplementation.TertiaryFixed;

    public HCTA TertiaryFixedDim => SchemeImplementation.TertiaryFixedDim;

    public HCTA OnTertiaryFixed => SchemeImplementation.OnTertiaryFixed;

    public HCTA OnTertiaryFixedVariant => SchemeImplementation.OnTertiaryFixedVariant;

    public event Action? OnUpdate
    {
        add => SchemeImplementation.OnUpdate += value;
        remove => SchemeImplementation.OnUpdate -= value;
    }
}