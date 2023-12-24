namespace MaterialDesign.Color.Colorspaces;

public interface IRGBAConvertible<out TSelf> where TSelf : IRGBAConvertible<TSelf>
{
    public RGBA ToRGBA();
    public static abstract TSelf FromRGBA(RGBA rgba);
}