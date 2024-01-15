using System.Diagnostics.CodeAnalysis;
using MaterialDesign.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace MaterialDesign.Web.Fonts;

internal class FontFaceCollection
{
    private Dictionary<string, List<FontFace>> FontsByClassName { get; } = [];
    private List<string> UniqueWeights { get; } = [];

    public FontFaceCollection()
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
    }
    
    public void Remove(string className, FontFace fontFace)
    {
        if (FontsByClassName.TryGetValue(className, out List<FontFace>? existingFontFaces))
            existingFontFaces.Remove(fontFace);
    }
    
    private class CollectionComponent : ComponentBase
    {
        [Inject, NotNull] 
        public FontFaceCollection? FontFaceCollection { get; set; }

        private Dictionary<string, List<FontFace>> FontsByClassName => FontFaceCollection.FontsByClassName;
        private List<string> UniqueWeights => FontFaceCollection.UniqueWeights;
        
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<DynamicHeadContent>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                int i = 0;
                childBuilder.OpenElement(i++, "style");
                    
                foreach (FontFace fontFace in FontsByClassName.Values.SelectMany(fontFaces => fontFaces)) 
                    childBuilder.AddContent(i++, (MarkupString)$"{fontFace}\n");
            
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
    }
}