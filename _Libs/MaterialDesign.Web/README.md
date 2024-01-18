Supplying some generic but highly useful components like `DynamicComponentContent` & `DynamicComponentOutlet`, and
`DynamicHeadContent` & `DynamicHeadOutlet`, which add the ability to essentially magic some razor stuff from one place
to another, `MaterialDesign.Web` is the base of the web-based projects under the `Nyan.MaterialDesign.*` libraries. 
If you're using this in WASM, the Material Library you're using it with should just set it up for you, 
but if you're on server then just put the `DynamicHeadOutlet` in your `App.razor` with `@rendermode="InteractiveServer"`
and it should work right.

It also adds support for dynamically-added fonts through `DynamicFont`, `DynamicFontCollection` 
(which allows the same font in different weights), and `ScopedFont` 
(which allows child content to be provided and the font to be applied to said child content).