using MaterialDesign.Colorspaces;

namespace MaterialDesignTesting;

[TestClass]
public class LABTests
{
    [TestMethod]
    public void ToWhiteRGBATest()
    {
        LAB lab = new LAB(100, 0, 0);

        RGBA actual = lab.ToRGBA();
        RGBA expected = new RGBA(255, 255, 255);

        Assert.AreEqual(expected.R, actual.R, 1);
        Assert.AreEqual(expected.G, actual.G, 1);
        Assert.AreEqual(expected.B, actual.B, 1);
    }

    [TestMethod]
    public void FromWhiteRGBATest()
    {
        RGBA rgba = new RGBA(255, 255, 255, 255);
        
        LAB actual = LAB.FromRGBA(rgba);
        LAB expected = new LAB(100, 0, 0);

        Assert.AreEqual(expected.L, actual.L, 1);
        Assert.AreEqual(expected.A, actual.A, 1);
        Assert.AreEqual(expected.B, actual.B, 1);
    }

    [TestMethod]
    public void ToRGBATest()
    {
        LAB lab = new(45, -40, 23);

        RGBA actual = lab.ToRGBA();
        RGBA expected = new(28, 122, 66);
        
        Assert.AreEqual(expected.R, actual.R, 1);
        Assert.AreEqual(expected.G, actual.G, 1);
        Assert.AreEqual(expected.B, actual.B, 1);
    }
    
    [TestMethod]
    public void FromRGBATest()
    {
        RGBA rgba = new(28, 122, 66);

        LAB actual = LAB.FromRGBA(rgba);
        LAB expected = new(45, -40, 23);
        
        Assert.AreEqual(expected.L, actual.L, 1);
        Assert.AreEqual(expected.A, actual.A, 1);
        Assert.AreEqual(expected.B, actual.B, 1);
    }
}