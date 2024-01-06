using MaterialDesign.Web.Components;

namespace MaterialDesign.Theming.Web.Setup;

public static class ThemeSetup
{
    /// <summary>
    /// Adds Web-specific Material Theming services and data as required.
    /// </summary>
    public static void Setup()
    {
        DynamicHeadOutlet.AddComponentSource<ThemeHeadContent>();
    }
}