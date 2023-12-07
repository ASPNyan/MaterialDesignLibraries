using MaterialDesign.Color.Colorspaces;

namespace MaterialDesignTesting;

[TestClass]
public class ColorTesting
{
    private const float Precision = 0.01f;
    
    [TestMethod]
    public void TestColorCanBeConvertedToInt()
    {
        var color = new Color(255, 128, 64, 50.0f);
        const uint expected = 4286595200;
        uint actual = (uint)color;
        
        Assert.AreEqual(expected, actual);
    }
        
    [TestMethod]
    public void TestColorCanBeConstructedFromInt()
    {
        const uint rep = 1090519039;
        var expected = new Color(64, 255, 255, 100f);
        var actual = (Color)rep;
        
        Assert.AreEqual(expected.R, actual.R);
        Assert.AreEqual(expected.G, actual.G);
        Assert.AreEqual(expected.B, actual.B);
        Assert.AreEqual(expected.A, actual.A, Precision);
    }
        
    [TestMethod]
    public void TestColorCreatesCorrectCopy()
    {
        var original = new Color(120, 45, 210, 77f);
        Color copy = original.Copy();
        
        Assert.AreEqual(original.R, copy.R);
        Assert.AreEqual(original.G, copy.G);
        Assert.AreEqual(original.B, copy.B);
        Assert.AreEqual(original.A, copy.A, Precision);
    }
        
    [TestMethod]
    public void TestColorEquality()
    {
        var color1 = new Color(255, 255, 0);
        var color2 = new Color(255, 255, 0);
        
        Assert.IsTrue(color1.Equals(color2));
    }
        
    [TestMethod]
    public void TestColorInequality()
    {
        var color1 = new Color(255, 255, 0);
        var color2 = new Color(255, 0, 0);
        
        Assert.IsFalse(color1.Equals(color2));
    }
    
    [TestMethod]
    public void TestRgbToHslConversion()
    {
        var color = new Color(255, 128, 0);

        // Approximate expected HSL values
        const float expectedH = 30f;
        const float expectedS = 100f;
        const float expectedL = 50f;

        Assert.AreEqual(expectedH, MathF.Round(color.H, 0, MidpointRounding.AwayFromZero));
        Assert.AreEqual(expectedS, MathF.Round(color.S, 0, MidpointRounding.AwayFromZero));
        Assert.AreEqual(expectedL, MathF.Round(color.L, 0, MidpointRounding.AwayFromZero));
    }

    [TestMethod]
    public void TestHslToRgbConversion()
    {
        var color = new Color(240f, 100f, 50f); // A blue color

        // Approximate RGB values
        const byte expectedR = 0;
        const byte expectedG = 0;
        const byte expectedB = 255;

        Assert.AreEqual(expectedR, color.R);
        Assert.AreEqual(expectedG, color.G);
        Assert.AreEqual(expectedB, color.B);
    }
}