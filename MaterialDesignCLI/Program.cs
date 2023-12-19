using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Common;
using MaterialDesign.Color.Contrast;
using MaterialDesign.Color.Schemes;
using MaterialDesignCLI;

const string clear = nameof(clear);
const string rgb = nameof(rgb);
const char r = 'r';
const char h = 'h';
const string hct = nameof(hct);
const string exit = nameof(exit);
const string createContrast = "createcontrast";
const string contrast = nameof(contrast);
const string scheme = nameof(scheme);
const string variants = nameof(variants);
const char hex = '#';

while (true)
{
    string? input;
    do input = Console.ReadLine()?.Trim().ToLower();
    while (input is null);

    switch (input)
    {
        case clear or clear + "()":
            Console.Clear();
            continue;
        case exit or exit + "()":
            return;
        case variants or variants + "()":
            string[] names = Enum.GetNames<Variant>();
            string output = names[0];
            output = names.Skip(1).Aggregate(output, (current, name) => current + ", " + name);
            Console.WriteLine(output);
            continue;
    }

    if (input is scheme)
    {
        Console.WriteLine("Scheme function syntax: scheme(HCTA source, Variant variant, Boolean isDark)");
        continue;
    }

    if (input.StartsWith(scheme))
    {
        Function schemeFunc = new(scheme, [GetHCTAFunc, GetSchemeVariant, GetBool]);
        try
        {
            List<object> outputs = schemeFunc.GetParameters(input);
            (HCTA color, Variant variant, bool isDark) = ((HCTA)outputs[0], (Variant)outputs[1], (bool)outputs[2]);
            DynamicScheme dynamicScheme = DynamicScheme.Create(color, variant, isDark);
            Console.WriteLine($"""
                               {dynamicScheme.Variant} Scheme - {(dynamicScheme.IsDark ? "Dark" : "Light")}
                               | Source: {dynamicScheme.Source}
                               | Primary: {dynamicScheme.Primary.GetWithTone(50)}
                               | Secondary: {dynamicScheme.Secondary.GetWithTone(50)}
                               | Tertiary: {dynamicScheme.Tertiary.GetWithTone(50)}
                               | Neutral: {dynamicScheme.Neutral.GetWithTone(50)}
                               | Neutral Variant: {dynamicScheme.NeutralVariant.GetWithTone(50)}
                               """);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        continue;
    }

    if (input is contrast)
    {
        Console.WriteLine("Contrast function syntax: contrast(HCT val1, HCT val2)");
        continue;
    }

    if (input.StartsWith(contrast))
    {
        Function contrastFunc = new(contrast, [GetHCTAFunc, GetHCTAFunc]);
        try
        {
            List<object> outputs = contrastFunc.GetParameters(input);
            (HCTA first, HCTA second) = ((HCTA)outputs[0], (HCTA)outputs[1]);
            Console.WriteLine($"Contrast: {Contrast.RatioOfTones(first.T, second.T)}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        continue;
    }

    if (input is createContrast)
    {
        Console.WriteLine("CreateContrast function syntax: createContrast(HCTA color, double ratio, bool darker)");
        continue;
    }
        
    if (input.StartsWith(createContrast))
    {
        Function createContrastFunc = new(createContrast, [GetHCTAFunc, GetDouble, GetBool]);
        try
        {
            List<object> outputs = createContrastFunc.GetParameters(input);
            (HCTA source, double ratio, bool darker) = ((HCTA)outputs[0], (double)outputs[1], (bool)outputs[2]);
            HCTA created = new(source.H, source.C,
                darker ? Contrast.DarkerViaRatio(source.T, ratio) : Contrast.LighterViaRatio(source.T, ratio));
            double createdContrast = Contrast.RatioOfTones(source.T, created.T);
            Console.WriteLine($"New: hcta({created.H:G5}, {created.C:G5}, {created.T:G5}, {created.A:G5}) " +
                              $"| Contrast: {createdContrast}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        continue;
    }

    if (!input.Contains("->"))
    {
        Console.WriteLine($"`{input}` is not a valid function.");
        continue;
    }
    
    string[] halves = input.Split("->", StringSplitOptions.TrimEntries);
    (string inputColor, string outputColor) = (halves[0], halves[1]);

    string typeofInput = string.Empty;
    if (inputColor.StartsWith(rgb)) typeofInput = rgb;
    else if (inputColor.StartsWith(hct)) typeofInput = hct;
    else if (inputColor.StartsWith(hex)) typeofInput += hex;
    else
    {
        Console.WriteLine($"{inputColor} is not a valid color input");
        continue;
    }

    string typeofOutput = string.Empty;
    if (outputColor.StartsWith(rgb)) typeofOutput = rgb;
    else if (outputColor.StartsWith(hct)) typeofOutput = hct;
    else if (outputColor.StartsWith(hex) || outputColor.StartsWith(nameof(hex))) typeofOutput += hex;
    else
    {
        Console.WriteLine($"{outputColor} is not a valid color input");
        continue;
    }
    
    HCTA hcta;
    
    switch (typeofInput[0], typeofOutput[0])
    {
        case (hex, hex):
        case (r, r):
        case (h, h):
            Console.WriteLine(inputColor);
            break;
        case (hex, r): 
            Console.WriteLine(Color.FromFormattedString(inputColor, WebFormat.AsCSS(WebFormat.Hex)).ToFormattedString(WebFormat.AsCSS(WebFormat.RGBA)));
            break;
        case (hex, h):
            hcta = HCTA.FromRGBA(Color.FromFormattedString(inputColor, WebFormat.AsCSS(WebFormat.Hex)));
            Console.WriteLine($"hcta({hcta.H:G5}, {hcta.C:G5}, {hcta.T:G5}, {hcta.A:G5})");
            break;
        case (r, hex):
            Console.WriteLine(Color.FromFormattedString(inputColor, WebFormat.AsCSS(WebFormat.RGBA)).ToFormattedString(WebFormat.AsCSS(WebFormat.Hex)));
            break;
        case (r, h):
            hcta = HCTA.FromRGBA(Color.FromFormattedString(inputColor, WebFormat.AsCSS(WebFormat.RGBA)));
            Console.WriteLine($"hcta({hcta.H:G5}, {hcta.C:G5}, {hcta.T:G5}, {hcta.A:G5})");
            break;
        case (h, hex):
            string[] valuesHex = inputColor[(inputColor.IndexOf('(') + 1)..^1].Split(',', StringSplitOptions.TrimEntries);
            hcta = new HCTA(double.Parse(valuesHex[0]), double.Parse(valuesHex[1]), double.Parse(valuesHex[2]));
            Console.WriteLine(((Color)hcta.ToRGBA()).ToFormattedString(WebFormat.AsCSS(WebFormat.Hex)));
            break;
        case (h, r):
            string[] valuesRGBA = inputColor[(inputColor.IndexOf('(') + 1)..^1].Split(',', StringSplitOptions.TrimEntries);
            hcta = new HCTA(double.Parse(valuesRGBA[0]), double.Parse(valuesRGBA[1]), double.Parse(valuesRGBA[2]));
            Console.WriteLine(((Color)hcta.ToRGBA()).ToFormattedString(WebFormat.AsCSS(WebFormat.RGBA)));
            break;
     }
}

bool GetHCTAFunc(string value, out object? output)
{
    if (!value.StartsWith(hct)) throw new Exception("Invalid syntax. Expected HCT color.");
            
    string[] hctValues = value[(value.IndexOf('(') + 1)..^1].Split(',', StringSplitOptions.TrimEntries);
    if (hctValues.Length is not 3 and not 4) 
        throw new Exception($"Invalid syntax. Expected 3 or 4 values and got {hctValues.Length}");
            
    (bool hPass, bool cPass, bool tPass) = (double.TryParse(hctValues[0], out double hVal), 
                                            double.TryParse(hctValues[1], out double cVal), 
                                            double.TryParse(hctValues[2], out double tVal));
    output = null;
    if (!hPass || !cPass || !tPass) return false;
    output = new HCTA(hVal, cVal, tVal);
    return true;
}

bool GetSchemeVariant(string value, out object? output)
{
    bool success = Enum.TryParse(value, true, out Variant val);
    output = success ? val : null;
    return success;
}

bool GetBool(string value, out object? output)
{
    bool success = bool.TryParse(value, out bool val);
    output = success ? val : null;
    return success;
}

bool GetDouble(string value, out object? output)
{
    bool success = double.TryParse(value, out double val);
    output = success ? val : null;
    return success;
}