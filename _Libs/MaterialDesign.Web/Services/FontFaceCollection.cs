using System.Diagnostics.CodeAnalysis;
using MaterialDesign.Web.Components;
using MaterialDesign.Web.Fonts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Web.Services;

internal class FontFaceCollection
{
    private Dictionary<string, List<FontFace>> FontsByClassName { get; } = [];
    private List<string> UniqueWeights { get; } = [];

    static FontFaceCollection()
    {
        DynamicHeadOutlet.AddComponentSource<CollectionComponent>();
    }
    
    public void Add(string className, FontFace fontFace)
    {
        if (FontsByClassName.TryGetValue(className, out List<FontFace>? existingFontFaces))
        {
            if (existingFontFaces.First().Family != fontFace.Family)
                throw new ArgumentException("The font family names of fonts under the same class name must match.");
            existingFontFaces.Add(fontFace);
        }
        else FontsByClassName.Add(className, [fontFace]);

        string weightClassString = fontFace.Weight.Replace(' ', '-');
        if (!UniqueWeights.Contains(weightClassString)) UniqueWeights.Add(weightClassString);
        
        OnUpdate?.Invoke();
    }
    
    public void Remove(string className, FontFace fontFace)
    {
        if (FontsByClassName.TryGetValue(className, out List<FontFace>? existingFontFaces))
            existingFontFaces.Remove(fontFace);

        if (existingFontFaces?.Count is 0) FontsByClassName.Remove(className);
        
        OnUpdate?.Invoke();
    }

    private event Action? OnUpdate;
    
    private class CollectionComponent : ComponentBase, IDisposable
    {
        [Inject, NotNull] 
        public IServiceProvider? ServiceProvider { get; set; }

        private FontFaceCollection FontFaceCollection { get; set; } = null!;
        private Dictionary<string, List<FontFace>> FontsByClassName => FontFaceCollection.FontsByClassName;
        private List<string> UniqueWeights => FontFaceCollection.UniqueWeights;

        private async void OnUpdate() => await InvokeAsync(StateHasChanged);

        private bool CanRender { get; set; } = true;
        protected override void OnInitialized()
        {
            FontFaceCollection? temp = ServiceProvider.GetService<FontFaceCollection>();
            if (temp is null)
            {
                CanRender = false;
                return;
            }
            FontFaceCollection = temp;
            FontFaceCollection.OnUpdate += OnUpdate;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (!CanRender) return;
            
            builder.OpenComponent<DynamicHeadContent>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                int i = 0;
                childBuilder.OpenElement(i++, "style");
                    
                foreach (FontFace fontFace in FontsByClassName.Values.SelectMany(fontFaces => fontFaces)) 
                    childBuilder.AddContent(i++, (MarkupString)$"{fontFace}");
            
                foreach ((string className, List<FontFace> fontFaces) in FontsByClassName)
                {
                    childBuilder.AddContent(i++, (MarkupString)$$"""
                                                            .{{className}} {
                                                              font-family: {{fontFaces.First().Family}};
                                                            }

                                                            """);
                }

                foreach (string weightValue in UniqueWeights)
                {
                    string className = FontWeight.ClassNameFromCSSValue(weightValue);
                    childBuilder.AddContent(i++, (MarkupString)$$"""
                                                                 .{{className}} {
                                                                   font-weight: {{weightValue}}
                                                                 }
                                                                 
                                                                 """);
                }
                
                childBuilder.CloseElement();
            }));
            
            builder.CloseComponent();
        }

        public void Dispose()
        {
            if (!CanRender) return;
            FontFaceCollection.OnUpdate -= OnUpdate;
        }
    }
}

/*
 * System.InvalidOperationException: Sequence contains no elements
 *     at System.Linq.ThrowHelper.ThrowNoElementsException()
 *     at System.Linq.Enumerable.First[TSource](IEnumerable`1 source)
 *     at MaterialDesign.Web.Services.FontFaceCollection.CollectionComponent.<BuildRenderTree>b__18_0(RenderTreeBuilder childBuilder)
 *     at Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder.AddContent(Int32 sequence, RenderFragment fragment)
 *     at MaterialDesign.Web.Components.DynamicHeadOutlet.BuildRenderTree(RenderTreeBuilder __builder)
 *     at Microsoft.AspNetCore.Components.Rendering.ComponentState.RenderIntoBatch(RenderBatchBuilder batchBuilder, RenderFragment renderFragment, Exception& renderFragmentException)
 */