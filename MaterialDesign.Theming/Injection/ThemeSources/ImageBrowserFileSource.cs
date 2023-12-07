using Microsoft.AspNetCore.Components.Forms;

namespace MaterialDesign.Theming.Injection.ThemeSources;

public class ImageBrowserFileSource : IAsyncDisposable
{
    private const string JpegMime = "image/jpeg";
    private const string PngMime = "image/png";
    private const string BmpMime = "image/bmp";
    private const string GifMime = "image/gif";
    private const string TiffMime = "image/tiff";

    public ImageSource Source { get; private set; } = new();

    public Task FromFile(IBrowserFile file, long maxAllowedFileSizeInBytes)
    {
        if (file.ContentType is not JpegMime and not PngMime and not BmpMime and not GifMime and not TiffMime)
            throw new InvalidOperationException($"{nameof(file)} must be a JPEG, PNG, BMP, GIF, or TIFF.");

        return Source.SetImageSource(file, maxAllowedFileSizeInBytes);
    }

    public void FromFileMethod(Func<Task<IBrowserFile>> fileMethod, long maxAllowedFileSizeInBytes)
    {
        Source.SetImageSourceMethod(GetStreamFromFile);
        
        return;

        async Task<Stream> GetStreamFromFile()
        {
            IBrowserFile file = await fileMethod();
            Stream stream = file.OpenReadStream(maxAllowedFileSizeInBytes);
            return stream;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Source.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}