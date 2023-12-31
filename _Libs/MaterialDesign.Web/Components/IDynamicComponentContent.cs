using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Components;

/// <summary>
/// Implementations of this that are <see cref="ComponentBase"/> inheriting should override
/// <see cref="ComponentBase.OnAfterRender"/> or <see cref="ComponentBase.OnAfterRenderAsync"/> and invoke the
/// <see cref="OnChange"/> event.
/// </summary>
public interface IDynamicComponentContent
{
    public static abstract event Action OnChange;
    public static abstract List<RenderFragment> GetContent();
}

/// <summary>
/// Implementations of this that are <see cref="ComponentBase"/> inheriting should override
/// <see cref="ComponentBase.OnAfterRender"/> or <see cref="ComponentBase.OnAfterRenderAsync"/> and invoke the
/// <see cref="OnChangeWithOutletId"/> event.
/// </summary>
public interface IDynamicIdComponentContent
{
    protected string OutletId { get; }

    public static abstract event Action<string> OnChangeWithOutletId;
    public static abstract List<RenderFragment>? GetContentWithOutletId(string id);
}