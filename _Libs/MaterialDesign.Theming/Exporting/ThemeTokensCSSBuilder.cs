namespace MaterialDesign.Theming.Exporting;

public class ThemeTokensCSSBuilder(Theme theme)
{
    private static RGBA RGBA(HCTA color) => color.ToRGBA();

    public string ThemeTokens()
    {
        bool wasDark = theme.IsDarkScheme;
        theme.SetLight();
        string val = $$"""
                       :root {
                         --md-sys-color-primary-light: {{RGBA(theme.Primary)}};
                         --md-sys-color-on-primary-light: {{RGBA(theme.OnPrimary)}};
                         --md-sys-color-primary-container-light: {{RGBA(theme.PrimaryContainer)}};
                         --md-sys-color-on-primary-container-light: {{RGBA(theme.OnPrimaryContainer)}};
                         --md-sys-color-secondary-light: {{RGBA(theme.Secondary)}};
                         --md-sys-color-on-secondary-light: {{RGBA(theme.OnSecondary)}};
                         --md-sys-color-secondary-container-light: {{RGBA(theme.SecondaryContainer)}};
                         --md-sys-color-on-secondary-container-light: {{RGBA(theme.OnSecondaryContainer)}};
                         --md-sys-color-tertiary-light: {{RGBA(theme.Tertiary)}};
                         --md-sys-color-on-tertiary-light: {{RGBA(theme.OnTertiary)}};
                         --md-sys-color-tertiary-container-light: {{RGBA(theme.TertiaryContainer)}};
                         --md-sys-color-on-tertiary-container-light: {{RGBA(theme.OnTertiaryContainer)}};
                         --md-sys-color-error-light: {{RGBA(theme.Error)}};
                         --md-sys-color-error-container-light: {{RGBA(theme.ErrorContainer)}};
                         --md-sys-color-on-error-light: {{RGBA(theme.OnError)}};
                         --md-sys-color-on-error-container-light: {{RGBA(theme.OnErrorContainer)}};
                         --md-sys-color-background-light: {{RGBA(theme.Background)}};
                         --md-sys-color-on-background-light: {{RGBA(theme.OnBackground)}};
                         --md-sys-color-surface-light: {{RGBA(theme.Surface)}};
                         --md-sys-color-on-surface-light: {{RGBA(theme.OnSurface)}};
                         --md-sys-color-surface-variant-light: {{RGBA(theme.SurfaceVariant)}};
                         --md-sys-color-on-surface-variant-light: {{RGBA(theme.OnSurfaceVariant)}};
                         --md-sys-color-outline-light: {{RGBA(theme.Outline)}};
                         --md-sys-color-inverse-on-surface-light: {{RGBA(theme.OnSurfaceInverse)}};
                         --md-sys-color-inverse-surface-light: {{RGBA(theme.SurfaceInverse)}};
                         --md-sys-color-inverse-primary-light: {{RGBA(new HCTA(theme.Primary.H, theme.Primary.C, 80))}};
                         --md-sys-color-shadow-light: {{new RGBA(0, 0, 0)}};
                         --md-sys-color-surface-tint-light: {{RGBA(theme.SurfaceVariant)}};
                         --md-sys-color-outline-variant-light: {{RGBA(theme.OutlineVariant)}};
                         --md-sys-color-scrim-light: {{new RGBA(0, 0, 0)}};
                       """;
        theme.SetDark();
        val += $$"""
                   --md-sys-color-primary-dark: {{RGBA(theme.Primary)}};
                   --md-sys-color-on-primary-dark: {{RGBA(theme.OnPrimary)}};
                   --md-sys-color-primary-container-dark: {{RGBA(theme.PrimaryContainer)}};
                   --md-sys-color-on-primary-container-dark: {{RGBA(theme.OnPrimaryContainer)}};
                   --md-sys-color-secondary-dark: {{RGBA(theme.Secondary)}};
                   --md-sys-color-on-secondary-dark: {{RGBA(theme.OnSecondary)}};
                   --md-sys-color-secondary-container-dark: {{RGBA(theme.SecondaryContainer)}};
                   --md-sys-color-on-secondary-container-dark: {{RGBA(theme.OnSecondaryContainer)}};
                   --md-sys-color-tertiary-dark: {{RGBA(theme.Tertiary)}};
                   --md-sys-color-on-tertiary-dark: {{RGBA(theme.OnTertiary)}};
                   --md-sys-color-tertiary-container-dark: {{RGBA(theme.TertiaryContainer)}};
                   --md-sys-color-on-tertiary-container-dark: {{RGBA(theme.OnTertiaryContainer)}};
                   --md-sys-color-error-dark: {{RGBA(theme.Error)}};
                   --md-sys-color-error-container-dark: {{RGBA(theme.ErrorContainer)}};
                   --md-sys-color-on-error-dark: {{RGBA(theme.OnError)}};
                   --md-sys-color-on-error-container-dark: {{RGBA(theme.OnErrorContainer)}};
                   --md-sys-color-background-dark: {{RGBA(theme.Background)}};
                   --md-sys-color-on-background-dark: {{RGBA(theme.OnBackground)}};
                   --md-sys-color-surface-dark: {{RGBA(theme.Surface)}};
                   --md-sys-color-on-surface-dark: {{RGBA(theme.OnSurface)}};
                   --md-sys-color-surface-variant-dark: {{RGBA(theme.SurfaceVariant)}};
                   --md-sys-color-on-surface-variant-dark: {{RGBA(theme.OnSurfaceVariant)}};
                   --md-sys-color-outline-dark: {{RGBA(theme.Outline)}};
                   --md-sys-color-inverse-on-surface-dark: {{RGBA(theme.OnSurfaceInverse)}};
                   --md-sys-color-inverse-surface-dark: {{RGBA(theme.SurfaceInverse)}};
                   --md-sys-color-inverse-primary-dark: {{RGBA(new HCTA(theme.Primary.H, theme.Primary.C, 80))}};
                   --md-sys-color-shadow-dark: {{new RGBA(0, 0, 0)}};
                   --md-sys-color-surface-tint-dark: {{RGBA(theme.SurfaceVariant)}};
                   --md-sys-color-outline-variant-dark: {{RGBA(theme.OutlineVariant)}};
                   --md-sys-color-scrim-dark: {{new RGBA(0, 0, 0)}};
                 }
                 """;

        if (wasDark) theme.SetDark();
        else theme.SetLight();
        return val;
    }
}