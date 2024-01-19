# Material Design Libraries
A collection of related [Material Design](https://m3.material.io) based .NET libraries.

Featuring image quantization, color schemes and theming, web theming, icons & symbols, 
and some surprisingly helpful generic web components, this group of Material Design Libraries has it all**. 

** Except for everything else it doesn't have, which is most things. Other than that (which isn't much), 
it's got it all.

Before you go looking through all the libraries, they can all be found on NuGet as well, however they are all prefixed with `Nyan.`, so `Nyan.MaterialDesign.Color` for example.

---

## MaterialDesign.Color
`MaterialDesign.Color` is the base library for almost every library (except for `MaterialDesign.Icons` and 
`MaterialDesign.Web`), providing colorspaces, tonal palettes, color schemes, and other assorted color-related 
methods and classes.

Assuming a root namespace of `MaterialDesign.Color`, color blending methods can be found at `Blend.Blend`,
all colorspaces can be found under `Colorspaces` (the most used being `HCTA` and `RGBA`, and then the generic `Color`),
color contrast related methods can be found at `Contrast.Contrast`, disliked color detection and fixing can be found
at `Disliked.Disliked`, color palettes via image quantization at `Image.FromImage`, color palettes under `Palettes`,
image quantization classes and structs are under `Quantize` (although these shouldn't be used directly), many different
color schemes under `Schemes`, color scoring (for image quantization) at `Score.Score`, and some color temperature
related stuff under `Temperature`.

That aside, this library is the main point of the collection, and is required for just about every project in this
collection unless you're only using the `MaterialDesign.Icons` and/or `MaterialDesign.Web` libraries.

Random note, this was what I was originally working on before I scrapped the entire project (apart from some 
implementations) and re-started it on 02/12/23 (dd/mm/yy), which is when the git start date of this project.
Which, yes, means I did this project in about a month if you take out days for Christmas and New Years.

## MaterialDesign.Color.Extensions
`MaterialDesign.Color.Extensions` is a much smaller library, just providing some minimal extensions to its parent
library `MaterialDesign.Color`. With some small additions, like Color Deficiency color filters, some `CorePalette`
extensions, other colorspace extensions, it's useful when you need it, and isn't too heavy.

## MaterialDesign.Icons
`MaterialDesign.Icons` was actually another project I had sitting around that I made before I started this whole thing,
but when I got to making all the web stuff, I added it in because it makes sense to have I guess? It did take a bit of
rewriting to bring it to a better standard and to interpolate what would be `MaterialDesign.Web` (and what was a
different part of `MaterialDesign.Theming.Web`).

It adds support for Material Icons (well, more formally Material *Symbols* but go figure) through the `MdIcon`
component under `MaterialDesign.Icons.Components`. It also allows for the use of cascading config with the use of the
`MdIconConfig` component under the same namespace. Extensions to `IServiceCollection` add easy access to set up.

## MaterialDesign.Theming
`MaterialDesign.Theming` adds support for Material Themes, adding DI (dependency injection) support, `ThemeSources`
and Theme Building, extensions for `IServiceCollection` and `IServiceProvider` for easy set up, `ThemeContainer`
can be accessed from injection and contains a Theme and the ability to use events for theme updates.

## MaterialDesign.Theming.Web
`MaterialDesign.Theming.Web` supplies some components and CSS support for using in web projects. It can be setup with
`Setup.ThemeSetup.Setup()`, and then you can follow the standard material web classes like `primary`, `on-secondary`,
and other classes. There will be a list of every class at the bottom of this readme.

## MaterialDesign.Theming.Web.MudBlazor
This library only has one component, and it's the `MdMudThemeProvider` which is to be used instead of the standard
`MudThemeProvider` to allow the use of Material Design themes in MudBlazor components. It should just be put in your
default layout somewhere, just ***not*** in the head or `DynamicHeadContent`.

## MaterialDesign.Web
Supplying some generic but highly useful components like `DynamicComponentContent` & `DynamicComponentOutlet`, and
`DynamicHeadContent` & `DynamicHeadOutlet`, which add the ability to essentially magic some razor stuff from one place
to another. If you're using this in WASM, the Material Library you're using it with should just set it up for you,
but if you're on server then just put the `DynamicHeadOutlet` in your `App.razor` with `@rendermode="InteractiveServer"`
and it should work right.

## MaterialDesign.Theming.Web CSS Classes
`.primary`, `.primary-text`, `.on-primary`, `.on-primary-text`, `.primary-container`, `.primary-container-text`,
`.on-primary-container`, `.on-primary-container-text`, `.secondary`, `.secondary-text`, `.on-secondary`,
`.on-secondary-text`, `.secondary-container`, `.secondary-container-text`, `.on-secondary-container`,
`.on-secondary-container-text`, `.tertiary`, `.tertiary-text`, `.on-tertiary`, `.on-tertiary-text`,
`.tertiary-container`, `.tertiary-container-text`, `.on-tertiary-container`, `.on-tertiary-container-text`, `.error`,
`.error-text`, `.on-error`, `.on-error-text`, `.error-container`, `.error-container-text`, `.on-error-container`,
`.on-error-container-text`, `.outline`, `.outline-text`, `.background`, `.background-text`, `.on-background`,
`.on-background-text`, `.surface`, `.surface-text`, `.on-surface`, `.on-surface-text`, `.surface-variant`,
`.surface-variant-text`, `.on-surface-variant`, `.on-surface-variant-text`, `.inverse-surface`, `.inverse-surface-text`,
`.inverse-on-surface`, `.inverse-on-surface-text`, `.inverse-primary`, `.inverse-primary-text`, `.shadow`,
`.shadow-text`, `.surface-tint`, `.surface-tint-text`,  `.outline-variant`, `.outline-variant-text`, `.scrim`,
`.scrim-text`, `.primary-fixed`, `.primary-fixed-text`, `.on-primary-fixed`, `.on-primary-fixed-text`,
`.secondary-fixed`, `.secondary-fixed-text`, `.on-secondary-fixed`, `.on-secondary-fixed-text`, `.tertiary-fixed`,
`.tertiary-fixed-text`, `.on-tertiary-fixed`, `.on-tertiary-fixed-text`, `.primary-fixed-dim`,
`.primary-fixed-dim-text`, `.secondary-fixed-dim`, `.secondary-fixed-dim-text`, `.tertiary-fixed-dim`,
`.tertiary-fixed-dim-text`, `.surface-dim`, `.surface-dim-text`, `.surface-bright`, `.surface-bright-text`,
`.surface-container-highest`, `.surface-container-highest-text`, `.surface-container-high`,
`.surface-container-high-text`, `.surface-container`, `.surface-container-text`, `.surface-container-low`,
`.surface-container-low-text`, `.surface-container-lowest`, `.surface-container-lowest-text`
