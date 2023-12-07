namespace MaterialDesign.Theming.Injection.ThemeSources;

public class ThemeSourceBuilder
{
    private ImageSourceBuilder? ImageBuilder { get; set; }
    private IThemeSource? ThemeSource { get; set; }

    public void UsingImage(Action<ImageSourceBuilder> method)
    {
        ImageBuilder = new ImageSourceBuilder();
        method(ImageBuilder);
    }

    public void WithColorMethod(Action<ColorMethodSource> method)
    {
        ColorMethodSource colorSource = new ColorMethodSource();
        method(colorSource);
        colorSource.CheckSourceGetter(true);
        ThemeSource = colorSource;
    }

    public void WithPresetColor(Action<PresetColorSource> method)
    {
        PresetColorSource colorSource = new();
        method(colorSource);
        ThemeSource = colorSource;
    }

    public IThemeSource Build()
    {
        if (ImageBuilder is null && ThemeSource is null) throw new ArgumentNullException(null,
            $"{nameof(ThemeSourceBuilder)} requires that its source be set with one of its `With` or `Using` methods.");

        return ImageBuilder?.GetImageSource() ?? ThemeSource!;
    }
    
    public class ImageSourceBuilder
    {
        private ImageSource? ImageSource { get; set; }

        public void WithBrowserFile(Action<ImageBrowserFileSource> method)
        {
            ImageBrowserFileSource fileSource = new();
            method(fileSource);
            ImageSource = fileSource.Source;
        }

        public void WithPath(Action<ImagePathSource> method)
        {
            ImagePathSource pathSource = new();
            method(pathSource);
            ImageSource = pathSource.Source;
        }

        public void WithStream(Action<ImageStreamSource> method)
        {
            ImageStreamSource streamSource = new();
            method(streamSource);
            ImageSource = streamSource.Source;
        }

        internal ImageSource GetImageSource()
        {
            ArgumentNullException.ThrowIfNull(ImageSource, nameof(ImageSource));
            return ImageSource;
        }
    }
}