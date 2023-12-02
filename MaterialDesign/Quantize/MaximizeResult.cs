namespace MaterialDesign.Quantize;

/// <summary>
/// Stores data for the result of the private <see cref="QuantizerWu.Maximize">QuantizerWu.Maximize</see> method.
/// </summary>
internal readonly record struct MaximizeResult(int CutLocation, double Maximum);