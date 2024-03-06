using System.Diagnostics.Contracts;
using MaterialDesign.Color.Quantize;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace MaterialDesign.Color.Image;

public static class FromImage
{
    /// <summary>
    /// Generates a list of the highest scored <see cref="CorePalette"/>s, with a max count of <paramref name="maxPaletteCount"/>.
    /// </summary>
    /// <param name="imageBytes">The bytes of the image</param>
    /// <param name="maxPaletteCount">The maximum amount of palettes to be generated</param>
    /// <returns>A list of the <see cref="CorePalette"/>s, in order of highest score to lower.</returns>
    [Pure]
    public static List<CorePalette> PalettesFromImage(in byte[] imageBytes, int maxPaletteCount = 5) => 
        PalettesFromImageWithScores(imageBytes, maxPaletteCount).Values;

    /// <summary>
    /// Generates a scored FrequencyMap with <see cref="CorePalette"/>s generated with a max count of <paramref name="maxPaletteCount"/>
    /// </summary>
    /// <param name="imageBytes">The bytes of the image</param>
    /// <param name="maxPaletteCount">The maximum amount of palettes to be generated</param>
    /// <returns>The scored FrequencyMap with a max of <paramref name="maxPaletteCount"/> palettes.</returns>
    [Pure]
    public static FrequencyMap<CorePalette, double> PalettesFromImageWithScores(byte[] imageBytes, int maxPaletteCount = 5)
    {
        RGBA[] rgbaValues = GetRGBAFromImageBytes(imageBytes).ToArray();

        FrequencyMap<RGBA> quantized = QuantizerCelebi.Quantize(rgbaValues, 256);

        FrequencyMap<HCTA> quantizedHCTA = FrequencyMap<HCTA>.From(quantized, HCTA.FromRGBA);

        FrequencyMap<HCTA, double> scores = Score.Score.Scored(quantizedHCTA, maxPaletteCount);

        int scoreCount = scores.Count > maxPaletteCount ? maxPaletteCount : scores.Count;

        return FrequencyMap<CorePalette, double>.From(scores.GetMostFrequentWithFrequencies(scoreCount),
            hcta => new CorePalette(hcta), freq => freq);
    }

    /// <summary>
    /// <inheritdoc cref="PalettesFromImageWithScores"/>
    /// </summary>
    /// <param name="imageStream">A stream containing the image.</param>
    /// <param name="maxPaletteCount"><inheritdoc cref="PalettesFromImageWithScores"/></param>
    /// <returns><inheritdoc cref="PalettesFromImageWithScores"/></returns>
    public static async Task<FrequencyMap<CorePalette, double>> PalettesFromImageStreamWithScores(Stream imageStream,
        int maxPaletteCount = 5)
    {
        await using MemoryStream ms = new();
        await imageStream.CopyToAsync(ms);
        byte[] imageBytes = ms.ToArray();
        return PalettesFromImageWithScores(imageBytes, maxPaletteCount);
    }

    /// <summary>
    /// <inheritdoc cref="PalettesFromImage"/>
    /// </summary>
    /// <param name="imageStream">A stream containing the image.</param>
    /// <param name="maxPaletteCount"><inheritdoc cref="PalettesFromImage"/></param>
    /// <returns><inheritdoc cref="PalettesFromImage"/></returns>
    public static async Task<List<CorePalette>> PalettesFromImageStream(Stream imageStream, int maxPaletteCount = 5) =>
        (await PalettesFromImageStreamWithScores(imageStream, maxPaletteCount)).Values;

    /// <summary>
    /// <inheritdoc cref="PalettesFromImageWithScores"/>
    /// </summary>
    /// <param name="imagePath">A path pointing to the image</param>
    /// <param name="maxPaletteCount"><inheritdoc cref="PalettesFromImageWithScores"/></param>
    /// <returns><inheritdoc cref="PalettesFromImageWithScores"/></returns>
    public static async Task<FrequencyMap<CorePalette, double>> PalettesFromImagePathWithScores(string imagePath,
        int maxPaletteCount = 5)
    {
        await using FileStream fs = new(imagePath, FileMode.Open, FileAccess.Read);
        return await PalettesFromImageStreamWithScores(fs, maxPaletteCount);
    }
    
    /// <summary>
    /// <inheritdoc cref="PalettesFromImage"/>
    /// </summary>
    /// <param name="file">A <see cref="IBrowserFile">BrowserFile</see> containing the image.</param>
    /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream.</param>
    /// <param name="maxPaletteCount"><inheritdoc cref="PalettesFromImage"/></param>
    /// <returns><inheritdoc cref="PalettesFromImage"/></returns>
    public static async Task<List<CorePalette>> PalettesFromWebImage(this IBrowserFile file, long maxAllowedSize, 
        int maxPaletteCount = 5)
    {
        await using Stream stream = file.OpenReadStream(maxAllowedSize);
        await using MemoryStream ms = new();
        await stream.CopyToAsync(ms);
        byte[] imageBytes = ms.ToArray();
        List<CorePalette> palettesFromImage = PalettesFromImage(imageBytes, maxPaletteCount);
        return palettesFromImage;
    }

    /// <summary>
    /// <inheritdoc cref="PalettesFromImageWithScores"/>
    /// </summary>
    /// <param name="file">A <see cref="IBrowserFile">BrowserFile</see> containing the image.</param>
    /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream.</param>
    /// <param name="maxPaletteCount"><inheritdoc cref="PalettesFromImageWithScores"/></param>
    /// <returns><inheritdoc cref="PalettesFromImageWithScores"/></returns>
    public static async Task<FrequencyMap<CorePalette, double>> PalettesFromWebImageWithScores(this IBrowserFile file, 
        long maxAllowedSize, int maxPaletteCount = 5)
    {
        await using Stream stream = file.OpenReadStream(maxAllowedSize);
        await using MemoryStream ms = new();
        await stream.CopyToAsync(ms);
        byte[] imageBytes = ms.ToArray();
        return PalettesFromImageWithScores(imageBytes.ToArray(), maxPaletteCount);
    }

    private static List<RGBA> GetRGBAFromImageBytes(in byte[] imageBytes)
    {
        using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(imageBytes);
        image.Mutate(context => context.Resize(new ResizeOptions
            { Size = new Size(128, 128), Mode = ResizeMode.Stretch, Sampler = new BicubicResampler() }));

        List<RGBA> rgbaValues = [];

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                Rgba32 pixel = image[x, y];
                rgbaValues.Add(new RGBA(pixel.R, pixel.G, pixel.B, pixel.A / 255f * 100f));
            }
        }

        return rgbaValues;
    }
}