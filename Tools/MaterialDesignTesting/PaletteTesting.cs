using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Palettes;

namespace MaterialDesignTesting;

[TestClass]
public class PaletteTesting
{
    [TestMethod]
    public void TestPalette()
    {
        RGBA color = new(0, 0, 255);

        TonalPalette palette = new(color);
        
        RGBA t100 = palette.GetWithTone(100).ToRGBA();
        RGBA t95 = palette.GetWithTone(95).ToRGBA();
        RGBA t90 = palette.GetWithTone(90).ToRGBA();
        RGBA t80 = palette.GetWithTone(80).ToRGBA();
        RGBA t70 = palette.GetWithTone(70).ToRGBA();
        RGBA t60 = palette.GetWithTone(60).ToRGBA();
        RGBA t50 = palette.GetWithTone(50).ToRGBA();
        RGBA t40 = palette.GetWithTone(40).ToRGBA();
        RGBA t30 = palette.GetWithTone(30).ToRGBA();
        RGBA t20 = palette.GetWithTone(20).ToRGBA();
        RGBA t10 = palette.GetWithTone(10).ToRGBA();
        RGBA t0 = palette.GetWithTone(0).ToRGBA();
        
        RGBAIsEqual((RGBA)0xffffffff, t100);
        RGBAIsEqual((RGBA)0xf1efffff, t95);
        RGBAIsEqual((RGBA)0xe0e0ffff, t90);
        RGBAIsEqual((RGBA)0xbec2ffff, t80);
        RGBAIsEqual((RGBA)0x9da3ffff, t70);
        RGBAIsEqual((RGBA)0x7c84ffff, t60);
        RGBAIsEqual((RGBA)0x5a64ffff, t50);
        RGBAIsEqual((RGBA)0x343dffff, t40);
        RGBAIsEqual((RGBA)0x0000efff, t30);
        RGBAIsEqual((RGBA)0x0001acff, t20);
        RGBAIsEqual((RGBA)0x00006eff, t10);
        RGBAIsEqual((RGBA)0x000000ff, t0);
    }

    private static void RGBAIsEqual(RGBA expected, RGBA actual)
    {
        Assert.AreEqual(expected.R, actual.R, 1);
        Assert.AreEqual(expected.G, actual.G, 1);
        Assert.AreEqual(expected.B, actual.B, 1);
    }
}