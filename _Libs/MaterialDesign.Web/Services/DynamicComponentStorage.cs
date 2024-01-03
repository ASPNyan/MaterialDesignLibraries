using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Services;

public class DynamicComponentStorage
{
    private Dictionary<string, List<RenderFragment>> ContentPerId { get; } = [];

    public bool GetContentFromId(string id, [NotNullWhen(true)] out List<RenderFragment>? content) => 
        ContentPerId.TryGetValue(id, out content);

    public void UpdateContentAtId(string id, RenderFragment content)
    {
        if (ContentPerId.TryGetValue(id, out var list)) list.Add(content);
        else ContentPerId[id] = [content];
        
        OnUpdateAtId?.Invoke(id);
    }

    public void RemoveFromId(string id, RenderFragment content)
    {
        if (ContentPerId.TryGetValue(id, out var list))
        {
            list.Remove(content);
            OnUpdateAtId?.Invoke(id);
        }
    }

    public event Action<string>? OnUpdateAtId;
}