using MaterialDesign.Color.Colorspaces;

namespace MaterialDesignTesting;

[TestClass]
public class HCTColorTesting
{
    private const double Precision = 5e-5;

    [TestMethod]
    public void TestToRGB()
    {
        HCTA hcta = new(0, 80, 60);

        Color rgba = hcta.ToRGBA();

        const byte r = 250;
        const byte g = 79;
        const byte b = 150;
        
        Assert.AreEqual(r, rgba.R, 1);
        Assert.AreEqual(g, rgba.G, 1);
        Assert.AreEqual(b, rgba.B, 1);
    }

    [TestMethod]
    public void TestFromRGB()
    {
        Color rgba = new(250, 79, 150);

        HCTA hcta = HCTA.FromRGBA(rgba);

        // values from material-color-utilities calculations
        const double h = 359.89236015938775;
        const double c = 80.28081404742244;
        const double t = 59.99516424325971;
        
        Assert.AreEqual(h, hcta.H, Precision);
        Assert.AreEqual(c, hcta.C, Precision);
        Assert.AreEqual(t, hcta.T, Precision);
    }

    [TestMethod]
    public void TestToFromRGB()
    {
        HCTA original = new(0, 80, 60);

        Color rgba = original.ToRGBA();

        HCTA converted = HCTA.FromRGBA(rgba);
        
        // rounding is required because HCT colors will not usually have exact RGB conversions, since HCT to RGB is lossy
        Assert.AreEqual(original.H, Math.Round(converted.H));
        Assert.AreEqual(original.C, Math.Round(converted.C));
        Assert.AreEqual(original.T, Math.Round(converted.T));
    }

    [TestMethod]
    public void TestFromToRGB()
    {
        Color original = new(0, 0, 255);

        HCTA hcta = HCTA.FromRGBA(original);

        Color converted = hcta.ToRGBA();
        
        Assert.AreEqual(original.R, converted.R, 1);
        Assert.AreEqual(original.G, converted.G, 1);
        Assert.AreEqual(original.B, converted.B, 1);
    }

    [TestMethod]
    public void TestHueWrapAround()
    {
        HCTA hcta = new(360, 100, 50);

        hcta.H += 30;
        
        Assert.AreEqual(30, hcta.H);

        hcta.H -= 200;
        
        Assert.AreEqual(190, hcta.H);
    }

    [TestMethod]
    public void TestToneExtremes()
    {
        HCTA white = new(0, 100, 100);
        HCTA black = new(0, 100, 0);

        RGBA whiteRGB = white.ToRGBA();
        RGBA blackRGB = black.ToRGBA();
        
        Assert.AreEqual((255, 255, 255), (whiteRGB.R, whiteRGB.G, whiteRGB.B));
        Assert.AreEqual((0, 0, 0), (blackRGB.R, blackRGB.G, blackRGB.B));
    }
}