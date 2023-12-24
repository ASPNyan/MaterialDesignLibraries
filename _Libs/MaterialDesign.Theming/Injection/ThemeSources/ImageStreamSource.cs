namespace MaterialDesign.Theming.Injection.ThemeSources;

public class ImageStreamSource : IAsyncDisposable
{
    public ImageSource Source { get; private set; } = new();

    public Task FromStream(Stream stream) => Source.SetImageSource(stream);

    public void FromStreamMethod(Func<Task<Stream>> method) => Source.SetImageSourceMethod(method);

    public async ValueTask DisposeAsync()
    {
        await Source.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}