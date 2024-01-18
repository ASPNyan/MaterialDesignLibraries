## MaterialDesign.Color
`MaterialDesign.Color` is the base library for almost every library under the Nyan.MaterialDesign.* collection (except for `MaterialDesign.Icons` and
`MaterialDesign.Web`), providing colorspaces, tonal palettes, color schemes, and other assorted color-related
methods and classes.

Assuming a root namespace of `MaterialDesign.Color`, color blending methods can be found at `Blend.Blend`,
all colorspaces can be found under `Colorspaces` (the most used being `HCTA` and `RGBA`, and then the generic `Color`),
color contrast related methods can be found at `Contrast.Contrast`, disliked color detection and fixing can be found
at `Disliked.Disliked`, color palettes via image quantization at `Image.FromImage`, color palettes under `Palettes`,
image quantization classes and structs are under `Quantize` (although these shouldn't be used directly), many different
color schemes under `Schemes`, color scoring (for image quantization) at `Score.Score`, and some color temperature
related stuff under `Temperature`.