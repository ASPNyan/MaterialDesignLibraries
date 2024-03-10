using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using SixLabors.ImageSharp;
using MaterialDesign.Color.Colorspaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ExampleSite.Data;

public class User : IdentityUser, IDisposable
{
    [Key]
    public override required string Id { get; set; }

    public HCTA Color = Program.DefaultColor;

    public async Task SetImageAsync(Stream imageStream)
    {
        if (ImageBytes is not null) Array.Clear(ImageBytes);
        
        Image image = await Image.LoadAsync(imageStream);

        using MemoryStream output = new();
        
        if (image.PixelType.AlphaRepresentation is null or PixelAlphaRepresentation.None)
        {
            image.Mutate(op =>
            {
                if (image is { Width: <= 815, Height: <= 815 }) return;

                if (image.Width > image.Height) op.Resize(815, 0);
                else op.Resize(0, 815);
            });

            await image.SaveAsJpegAsync(output);
            ImageMimeType = MediaTypeNames.Image.Jpeg;
        }
        else
        {
            image.Mutate(op =>
            {
                if (image is { Width: <= 700, Height: <= 700 }) return;

                if (image.Width > image.Height) op.Resize(700, 0);
                else op.Resize(0, 700);
            });

            await image.SaveAsPngAsync(output);
            ImageMimeType = MediaTypeNames.Image.Png;
        }

        ImageBytes = output.ToArray();
    }

    /// <summary>
    /// Max size is 2MB.
    /// </summary>
    [MaxLength(2_000_000, ErrorMessage = "User's Image may not exceed 2MB (about 700x700 with Alpha & 815x815 without.)")]
    public byte[]? ImageBytes;

    /// <summary>
    /// Set with <see cref="System.Net.Mime.MediaTypeNames.Image"/>.
    /// </summary>
    [MaxLength(15, ErrorMessage = "ImageMimeType may not exceed 15 characters in length.")]
    public string? ImageMimeType;

    public string? GenerateImageDataString() => ImageBytes is not null && ImageMimeType is not null
        ? $"data:{ImageMimeType};base64,{Convert.ToBase64String(ImageBytes)}"
        : null;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        if (ImageBytes is not null) Array.Clear(ImageBytes);
    }
}