using MaterialDesign.Icons.Exceptions;
using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Icons.Common.Components;

public sealed class DynamicComponentContent : ComponentBase, IDisposable
{
    private static Dictionary<string, List<RenderFragment>> ContentPerId { get; } = [];

    public static event Action<string>? OnChangeWithId;

    public static List<RenderFragment> GetContent(string id) => ContentPerId[id];
    
    [Parameter, EditorRequired]
    public string? Id { get; set; }
    
    [Parameter, EditorRequired]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        if (Id is null) ArgumentException.ThrowIfNullOrWhiteSpace(Id);
        if (ChildContent is null) return;

        if (!DynamicComponentOutlet.ExistingOutlets.Contains(Id))
            throw new ComponentException($"Outlet for component id `{Id}` does not exist.");
        
        if (ContentPerId.TryGetValue(Id, out var list)) list.Add(ChildContent);
        else ContentPerId[Id] = [ChildContent];
    }
    
    public void Dispose()
    {
        if (Id is null || ChildContent is null) return;
        ContentPerId[Id].Remove(ChildContent);
        OnChangeWithId?.Invoke(Id);
    }
}