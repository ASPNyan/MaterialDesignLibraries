﻿using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Theming;

public readonly struct Scheme(TonalPalette primary, TonalPalette secondary, TonalPalette tertiary, TonalPalette neutral,
    TonalPalette neutralVariant, bool isDark)
{
    public (TonalPalette Primary, TonalPalette Secondary, TonalPalette Tertiary, TonalPalette Neutral,
        TonalPalette NeutralVariant) Sources { get; } = (primary, secondary, tertiary, neutral, neutralVariant);

    private static HCTA FromPalette(TonalPalette palette) => new(palette.Hue, palette.Chroma, 50);
    
    private int Core => isDark ? 80 : 40;
    private int OnCore => Core + SignViaDark(60);
    private int CoreContainer => Core + SignViaDark(50);
    private int OnCoreContainer => isDark ? 90 : 10;
    
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

    public HCTA Outline => neutralVariant.GetWithTone(isDark ? 60 : 50);
    public HCTA OutlineVariant => neutralVariant.GetWithTone(isDark ? 80 : 30);
    
    public HCTA Background => neutral.GetWithTone(isDark ? 98 : 6);
    public HCTA OnBackground => neutral.GetWithTone(isDark ? 90 : 10);
    public HCTA Surface => Background;
    public HCTA OnSurface => OnBackground;
    public HCTA SurfaceVariant => neutralVariant.GetWithTone(isDark ? 30 : 90);
    public HCTA OnSurfaceVariant => neutralVariant.GetWithTone(isDark ? 80 : 30);
    
    public HCTA SurfaceInverse => neutral.GetWithTone(isDark ? 90 : 20);
    public HCTA OnSurfaceInverse => neutral.GetWithTone(isDark ? 20 : 95);
    public HCTA SurfaceBright => neutral.GetWithTone(isDark ? 24 : 98);
    public HCTA SurfaceDim => neutral.GetWithTone(isDark ? 6 : 87);
    
    public HCTA SurfaceContainer => neutral.GetWithTone(isDark ? 12 : 94);
    public HCTA SurfaceContainerLow => neutral.GetWithTone(isDark ? 10 : 96);
    public HCTA SurfaceContainerLowest => neutral.GetWithTone(isDark ? 4 : 100);
    public HCTA SurfaceContainerHigh => neutral.GetWithTone(isDark ? 17 : 92);
    public HCTA SurfaceContainerHighest => neutral.GetWithTone(isDark ? 22 : 90);

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
    
    private int SignViaDark(int tone) => isDark ? -tone : tone;
}