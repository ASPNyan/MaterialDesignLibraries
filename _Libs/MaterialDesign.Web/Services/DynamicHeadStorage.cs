using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Services;

public class DynamicHeadStorage
{
    private List<RenderFragment> Content { get; } = [];

    public List<RenderFragment> GetContent() => Content;

    public void UpdateContent(RenderFragment content)
    {
        Content.Add(content);
        OnUpdate?.Invoke();
    }

    public void Remove(RenderFragment content)
    {
        bool updated = Content.Remove(content);
        if (updated) OnUpdate?.Invoke();
    }

    public void InvokeUpdate() => OnUpdate?.Invoke();
    
    public event Action? OnUpdate;
}