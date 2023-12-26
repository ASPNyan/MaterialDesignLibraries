using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Icons.Common.Components;

public interface IDynamicComponentContent
{
    public static abstract event Action OnChange;
    public static abstract List<RenderFragment> GetContent();
}

public interface IDynamicIdComponentContent
{
    protected string OutletId { get; }

    public static abstract event Action<string> OnChangeWithOutletId;
    public static abstract List<RenderFragment> GetContentWithOutletId(string id);
}