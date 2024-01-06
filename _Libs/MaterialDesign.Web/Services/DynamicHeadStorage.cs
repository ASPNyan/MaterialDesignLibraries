using MaterialDesign.Web.Components;
using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Services;

/// <summary>
/// Adds the ability to have storage for <see cref="DynamicHeadOutlet"/> and <see cref="DynamicHeadContent"/>
/// components.
/// </summary>
public class DynamicHeadStorage
{
    private List<RenderFragment> Content { get; } = [];

    /// <summary>
    /// Gets the <see cref="RenderFragment"/>s from the storage.
    /// </summary>
    public List<RenderFragment> GetContent() => Content;

    /// <summary>
    /// Adds new content in the storage.
    /// </summary>
    public void UpdateContent(RenderFragment content)
    {
        Content.Add(content);
        OnUpdate?.Invoke();
    }

    /// <summary>
    /// Removes the specified <see cref="RenderFragment"/> from the storage.
    /// </summary>
    public void Remove(RenderFragment content)
    {
        bool updated = Content.Remove(content);
        if (updated) OnUpdate?.Invoke();
    }

    /// <summary>
    /// Invokes the <see cref="OnUpdate"/> event.
    /// </summary>
    public void InvokeUpdate() => OnUpdate?.Invoke();
    
    public event Action? OnUpdate;
}