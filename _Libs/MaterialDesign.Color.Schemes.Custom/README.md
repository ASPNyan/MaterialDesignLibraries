`MaterialDesign.Color.Schemes.Custom` adds support for creating custom color schemes and themes with customized rules.
These custom schemes aren't just custom colors, but custom rules, like the gap between core and on-core colors,
the gap between light and dark mode schemes, custom handling of secondary, tertiary, and surface color generation,
and more.

To create these custom schemes, create a new `record` inheriting from `CustomSchemeBase`, and then define all the
abstract properties, alongside overriding the virtual ones — should you wish to use them. Then, to construct the scheme,
create a constructor using the `base(HCTA)` constructor, and that should let you access all the colors from the record
successfully. Using primary constructor syntax, the start of your class could look like:

```csharp
using MaterialDesign.Color.Schemes.Custom;

public sealed record CustomScheme(HCTA Source) : CustomSchemeBase(Source)
{
    // implementations
}
```

Additionally, you can use extensions to `IServiceCollection` and then use the `ThemeCollection` class in DI that 
supports `IScheme` classes (which CustomSourceBase inherits from), so you can use that instead of a regular `Theme`
(which is still a great choice for ease-of-use, or if you don't need the extra customization).

For schemes that can be changed at runtime, the `ModifiableCustomScheme` is an option in that case. 
Additionally, on the `ModifiableCustomScheme`, changing the Hue, Chroma, and Tone properties on the instance
instead of modifying the Origin is recommended. Otherwise, using the Update() method instead would be the next option.