namespace MaterialDesign.Color.Schemes;

public readonly struct Scheme(CorePalette palette, bool isDark)
{
    public CorePalette Palette { get; } = palette;
    public bool IsDark { get; } = isDark;
    
    public HCTA Primary { get; } = palette.Primary.GetWithTone(isDark ? 80 : 40);
    public HCTA PrimaryContent { get; } = palette.PrimaryContent.GetWithTone(isDark ? 80 : 40);
    public HCTA OnPrimary { get; } = palette.Primary.GetWithTone(isDark ? 20 : 100);
    public HCTA OnPrimaryContent { get; } = palette.PrimaryContent.GetWithTone(isDark ? 20 : 100);
    public HCTA PrimaryContainer { get; } = palette.Primary.GetWithTone(isDark ? 30 : 90);
    public HCTA PrimaryContainerContent { get; } = palette.PrimaryContent.GetWithTone(isDark ? 30 : 90);
    public HCTA OnPrimaryContainer { get; } = palette.Primary.GetWithTone(isDark ? 90 : 10);
    public HCTA OnPrimaryContainerContent { get; } = palette.PrimaryContent.GetWithTone(isDark ? 90 : 10);
    
    public HCTA Secondary { get; } = palette.Secondary.GetWithTone(isDark ? 80 : 40);
    public HCTA SecondaryContent { get; } = palette.SecondaryContent.GetWithTone(isDark ? 80 : 40);
    public HCTA OnSecondary { get; } = palette.Secondary.GetWithTone(isDark ? 20 : 100);
    public HCTA OnSecondaryContent { get; } = palette.SecondaryContent.GetWithTone(isDark ? 20 : 100);
    public HCTA SecondaryContainer { get; } = palette.Secondary.GetWithTone(isDark ? 30 : 90);
    public HCTA SecondaryContainerContent { get; } = palette.SecondaryContent.GetWithTone(isDark ? 30 : 90);
    public HCTA OnSecondaryContainer { get; } = palette.Secondary.GetWithTone(isDark ? 90 : 10);
    public HCTA OnSecondaryContainerContent { get; } = palette.SecondaryContent.GetWithTone(isDark ? 90 : 10);
    
    public HCTA Tertiary { get; } = palette.Tertiary.GetWithTone(isDark ? 80 : 40);
    public HCTA TertiaryContent { get; } = palette.TertiaryContent.GetWithTone(isDark ? 80 : 40);
    public HCTA OnTertiary { get; } = palette.Tertiary.GetWithTone(isDark ? 20 : 100);
    public HCTA OnTertiaryContent { get; } = palette.TertiaryContent.GetWithTone(isDark ? 20 : 100);
    public HCTA TertiaryContainer { get; } = palette.Tertiary.GetWithTone(isDark ? 30 : 90);
    public HCTA TertiaryContainerContent { get; } = palette.TertiaryContent.GetWithTone(isDark ? 30 : 90);
    public HCTA OnTertiaryContainer { get; } = palette.Tertiary.GetWithTone(isDark ? 90 : 10);
    public HCTA OnTertiaryContainerContent { get; } = palette.TertiaryContent.GetWithTone(isDark ? 90 : 10);
    
    public HCTA Error { get; } = palette.Error.GetWithTone(isDark ? 80 : 40);
    public HCTA OnError { get; } = palette.Error.GetWithTone(isDark ? 20 : 100);
    public HCTA ErrorContainer { get; } = palette.Error.GetWithTone(isDark ? 30 : 90);
    public HCTA OnErrorContainer { get; } = palette.Error.GetWithTone(isDark ? 80 : 10);

    public HCTA Background { get; } = palette.Neutral.GetWithTone(isDark ? 10 : 99);
    public HCTA BackgroundContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 10 : 99);
    public HCTA OnBackground { get; } = palette.Neutral.GetWithTone(isDark ? 90 : 10);
    public HCTA OnBackgroundContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 90 : 10);
    
    public HCTA Surface { get; } = palette.Neutral.GetWithTone(isDark ? 10 : 99);
    public HCTA SurfaceContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 10 : 99);
    public HCTA OnSurface { get; } = palette.Neutral.GetWithTone(isDark ? 90 : 10);
    public HCTA OnSurfaceContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 90 : 10);
    
    public HCTA SurfaceVariant { get; } = palette.NeutralVariant.GetWithTone(isDark ? 30 : 90);
    public HCTA SurfaceVariantContent { get; } = palette.NeutralVariantContent.GetWithTone(isDark ? 30 : 90);
    public HCTA OnSurfaceVariant { get; } = palette.NeutralVariant.GetWithTone(isDark ? 80 : 30);
    public HCTA OnSurfaceVariantContent { get; } = palette.NeutralVariantContent.GetWithTone(isDark ? 80 : 30);
    
    public HCTA Outline { get; } = palette.NeutralVariant.GetWithTone(isDark ? 60 : 50);
    public HCTA OutlineContent { get; } = palette.NeutralVariantContent.GetWithTone(isDark ? 60 : 50);
    public HCTA OutlineVariant { get; } = palette.NeutralVariant.GetWithTone(isDark ? 30 : 80);
    public HCTA OutlineVariantContent { get; } = palette.NeutralVariantContent.GetWithTone(isDark ? 30 : 80);
    
    public HCTA Shadow { get; } = palette.Neutral.GetWithTone(0);
    public HCTA Scrim { get; } = palette.Neutral.GetWithTone(0);
    
    public HCTA InverseSurface { get; } = palette.Neutral.GetWithTone(isDark ? 90 : 20);
    public HCTA InverseSurfaceContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 90 : 20);
    public HCTA InverseOnSurface { get; } = palette.Neutral.GetWithTone(isDark ? 20 : 95);
    public HCTA InverseOnSurfaceContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 20 : 95);
    public HCTA InversePrimary { get; } = palette.Neutral.GetWithTone(isDark ? 40 : 80);
    public HCTA InversePrimaryContent { get; } = palette.NeutralContent.GetWithTone(isDark ? 40 : 80);
}