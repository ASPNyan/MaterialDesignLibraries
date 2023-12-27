using MaterialDesign.Web.Components;

namespace MaterialDesign.Theming.Web.Setup;

public static class ThemeSetup
{
    public static void Setup()
    {
        DynamicHeadOutlet.AddComponentSource<ThemeHeadContent>();
    }
}