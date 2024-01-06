using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using MaterialDesign.Web.Components;

namespace MaterialDesign.Web.Services;

/// <summary>
/// Adds the ability to have storage for <see cref="DynamicComponentOutlet"/> and <see cref="DynamicComponentContent"/>
/// components.
/// </summary>
public class DynamicComponentStorage
{
    private Dictionary<string, List<RenderFragment>> ContentPerId { get; } = [];

    /// <summary>
    /// Gets the <see cref="RenderFragment"/>s at the string-based id in the storage.
    /// </summary>
    public bool GetContentFromId(string id, [NotNullWhen(true)] out List<RenderFragment>? content) =>
        ContentPerId.TryGetValue(id, out content);

    /// <summary>
    /// Adds new content at the string-based id in the storage.
    /// </summary>
    public void UpdateContentAtId(string id, RenderFragment content)
    {
        if (ContentPerId.TryGetValue(id, out var list)) list.Add(content);
        else ContentPerId[id] = [content];
        
        OnUpdateAtId?.Invoke(id);
    }

    /// <summary>
    /// Removes the specified <see cref="RenderFragment"/> at the provided id in the storage.
    /// </summary>
    public void RemoveFromId(string id, RenderFragment content)
    {
        if (ContentPerId.TryGetValue(id, out var list))
        {
            list.Remove(content);
            OnUpdateAtId?.Invoke(id);
        }
    }

    /// <summary>
    /// Invokes the <see cref="OnUpdateAtId"/> event with the provided id.
    /// </summary>
    public void InvokeUpdate(string id) => OnUpdateAtId?.Invoke(id);
    
    public event Action<string>? OnUpdateAtId;
}