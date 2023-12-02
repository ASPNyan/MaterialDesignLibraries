namespace MaterialDesign.Common;

public interface IWebFormattable<out T> where T : IWebFormattable<T>
{
    public string ToFormattedString(WebFormat format);
    public static abstract T FromFormattedString(string value, WebFormat? format = null);
    public WebFormat[] ValidFormats { get; }
}