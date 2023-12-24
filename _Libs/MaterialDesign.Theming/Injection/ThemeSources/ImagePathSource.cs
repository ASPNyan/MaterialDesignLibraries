namespace MaterialDesign.Theming.Injection.ThemeSources;

public class ImagePathSource : IAsyncDisposable
{
    public ImageSource Source { get; private set; } = new();

    public Task FromPath(string path) => Source.SetImageSource(path);

    public void FromPathMethod(Func<Task<string>> method)
    {
        Source.SetImageSourceMethod(GetStreamFromPath);

        return;

        async Task<Stream> GetStreamFromPath()
        {
            FileStream stream = new(await method(), FileMode.Open, FileAccess.Read);
            return stream;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Source.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public static implicit operator ImageSource(ImagePathSource source) => source.Source;
}