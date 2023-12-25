using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Icons.Common.Components;

public sealed class DynamicComponentOutlet : ComponentBase, IDisposable
{
    internal static List<string> ExistingOutlets { get; } = [];
    
    [Parameter, EditorRequired]
    public string? Id { get; set; }

    private RenderFragment? _renderContent;

    protected override void OnInitialized()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Id);
        DynamicComponentContent.OnChangeWithId += UpdateState;
        ExistingOutlets.Add(Id);
    }

    private async void UpdateState(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Id);
        await InvokeAsync(() =>
        {
            _renderContent = builder =>
            {
                List<RenderFragment> fragments = DynamicComponentContent.GetContent(Id);
                for (int i = 0; i < fragments.Count; i++) builder.AddContent(i, fragments[i]);
            };
            
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        ArgumentException.ThrowIfNullOrEmpty(Id); // todo: match head outlet
        ExistingOutlets.Remove(Id);
    }
}