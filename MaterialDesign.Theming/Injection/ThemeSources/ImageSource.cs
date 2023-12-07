using Microsoft.AspNetCore.Components.Forms;

namespace MaterialDesign.Theming.Injection.ThemeSources;

public sealed class ImageSource : IThemeSource, IDisposable, IAsyncDisposable
{
    private Stream? Stream { get; set; }
    private Func<Task<Stream>>? StreamMethod { get; set; }
    private bool _disposed;

    public void SetImageSourceMethod(Func<Task<Stream>> method)
    {
        ObjectDisposedException.ThrowIf(_disposed, typeof(ImageSource));
        StreamMethod = method;
        Stream = null;
    }
    
    public async Task SetImageSource(string path)
    {
        ObjectDisposedException.ThrowIf(_disposed, typeof(ImageSource));
        StreamMethod = null;
        Stream = Stream.Null;
        await using FileStream source = new(path, FileMode.Open, FileAccess.Read);
        await source.CopyToAsync(Stream);
    }

    public Task SetImageSource(Stream source)
    {
        ObjectDisposedException.ThrowIf(_disposed, typeof(ImageSource));
        StreamMethod = null;
        Stream = Stream.Null;
        return source.CopyToAsync(Stream);
    }

    public async Task SetImageSource(IBrowserFile file, long maxAllowedBytes)
    {
        ObjectDisposedException.ThrowIf(_disposed, typeof(ImageSource));
        StreamMethod = null;
        Stream = Stream.Null;
        await using Stream source = file.OpenReadStream(maxAllowedBytes);
        await source.CopyToAsync(Stream);
    }

    async Task<HCTA> IThemeSource.GetSource()
    {
        ObjectDisposedException.ThrowIf(_disposed, typeof(ImageSource));
        if (Stream is null && StreamMethod is null) throw new ArgumentNullException(null,
                $"{nameof(ImageSource)} requires that it be set with one of its `Set` methods.");
        
        return (await Color.Image.FromImage.PalettesFromImageStream(Stream ?? await StreamMethod!(), 1))
            .First().Origin;
    }

    ~ImageSource()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && Stream is not null)
            {
                Stream.Dispose();
                Stream = null;
            }
            
            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Stream is not null) await Stream.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}