`MaterialDesign.Icons` was actually another project I had sitting around that I made before I started this whole thing,
but when I got to making all the web stuff, I added it in because it makes sense to have I guess? It did take a bit of
rewriting to bring it to a better standard and to interpolate what would be `MaterialDesign.Web` (and what was a
different part of `MaterialDesign.Theming.Web`).

It adds support for Material Icons (well, more formally Material *Symbols* but go figure) through the `MdIcon`
component under `MaterialDesign.Icons.Components`. It also allows for the use of cascading config with the use of the
`MdIconConfig` component under the same namespace. Extensions to `IServiceCollection` add easy access to set up.