using MudBlazor;
using MudBlazor.Utilities;
using MaterialDesign.Color.Colorspaces;
using Microsoft.AspNetCore.Components;
using MaterialDesign.Theming.Injection;
using Microsoft.AspNetCore.Components.Rendering;

namespace MaterialDesign.Theming.Web.MudBlazor;

/// <summary>
/// Adds on to MudBlazor's <see cref="MudThemeProvider"/> to add support for Material Design <see cref="Theme"/> in
/// MudBlazor components. Should be used instead of <see cref="MudThemeProvider"/>, not alongside it.
/// </summary>
public class MdMudThemeProvider : ComponentBase
{
#nullable disable
    [Inject] public ThemeContainer ThemeContainer { get; set; }
#nullable restore
    
    private IScheme MdTheme => ThemeContainer.Scheme;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<MudThemeProvider>(0);
        builder.AddAttribute(1, "Theme", GetMudTheme());
        builder.CloseComponent();
    }

    private static MudColor MudColor(HCTA hcta)
    {
        RGBA rgba = hcta.ToRGBA();
        return new MudColor(rgba.R, rgba.G, rgba.B, rgba.A);
    }
    
    private MudTheme GetMudTheme()
    {
        PaletteDark dark = new()
        {
            Primary = MudColor(MdTheme.Primary),
            TextPrimary = MudColor(MdTheme.OnPrimary),
            Secondary = MudColor(MdTheme.Secondary),
            TextSecondary = MudColor(MdTheme.OnSecondary),
            Tertiary = MudColor(MdTheme.Tertiary),
            Error = MudColor(MdTheme.Error),
            Background = MudColor(MdTheme.Background),
            Surface = MudColor(MdTheme.Surface),
        };
        
        PaletteLight light = new()
        {
            Primary = MudColor(MdTheme.Primary),
            TextPrimary = MudColor(MdTheme.OnPrimary),
            Secondary = MudColor(MdTheme.Secondary),
            TextSecondary = MudColor(MdTheme.OnSecondary),
            Tertiary = MudColor(MdTheme.Tertiary),
            Error = MudColor(MdTheme.Error),
            Background = MudColor(MdTheme.Background),
            Surface = MudColor(MdTheme.Surface),
        };

        return new MudTheme
        {
            PaletteLight = light,
            PaletteDark = dark
        }; // these are the same because the ThemeContainer will handle the coloring itself on rerender.
    }
    
    private async void OnThemeUpdate() => await InvokeAsync(StateHasChanged);

    protected override void OnInitialized()
    {
        ThemeContainer.OnSchemeUpdate += OnThemeUpdate;
    }
}