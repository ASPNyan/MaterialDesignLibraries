namespace MaterialDesign.Quantize;

/// <summary>
/// An image quantizer that improves on the quality of a standard K-Means
/// algorithm by setting the K-Means initial state to the output of a Wu
/// quantizer, instead of random centroids. Improves on speed by several
/// optimizations, as implemented in WsMeans, or Weighted Square Means, K-Means
/// with those optimizations.
///
/// This algorithm was designed by M. Emre Celebi, and was found in their 2011
/// paper, Improving the Performance of K-Means for Color Quantization.
/// https://arxiv.org/abs/1101.0395
/// </summary>
public static class QuantizerCelebi
{
    /// <summary>
    /// Quantizes an array of <see cref="RGBA"/> pixels to create a <see cref="FrequencyMap{TValue}"/> of at most
    /// <paramref name="maxColors"/> colors.
    /// </summary>
    /// <param name="pixels">The array of <see cref="RGBA"/> pixels to quantize.</param>
    /// <param name="maxColors">The most colors the method should return in total.</param>
    /// <returns>A <see cref="FrequencyMap{TValue}"/> of the quantized colors, with a max
    /// <see cref="FrequencyMap{TValue}.FrequencySum"/> of <paramref name="maxColors"/></returns>
    public static FrequencyMap<RGBA> Quantize(in RGBA[] pixels, int maxColors)
    {
        RGBA[] cleansedPixels = pixels.Where(rgba => rgba.A >= 80).ToArray(); // clear translucent & transparent pixels

        QuantizerWu wu = new();
        RGBA[] wuResult = wu.Quantize(cleansedPixels, maxColors);
        FrequencyMap<RGBA> wsResult = QuantizerWsMeans.Quantize(cleansedPixels, wuResult, maxColors);
        return wsResult;
    }
}