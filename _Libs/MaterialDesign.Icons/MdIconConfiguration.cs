namespace MaterialDesign.Icons;

/// <summary>
/// A configuration for Material Icons, used in <see cref="Components.MdIconConfig"/>.
/// </summary>
public class MdIconConfiguration
{
    public bool IsStatic { get; }
    public bool Fill { get; private set; }
    public int Weight { get; private set; }
    public int Grade { get; private set; }
    public int OpticalSize { get; private set; }

    private const bool Static = true;
    private const bool Dynamic = false;

    private MdIconConfiguration(bool isStatic, bool fill, int weight, int grade, int opticalSize)
    {
        Fill = fill;
        Weight = weight;
        Grade = grade;
        OpticalSize = opticalSize;
        IsStatic = isStatic;
    }
    
    /// <summary>
    /// Creates a Material Icon configuration with pre-defined static values.
    /// </summary>
    public static MdIconConfiguration CreateStatic(bool fill = false, int weight = 400,
        int grade = 0, int opticalSize = 24) 
            => new(Static, fill, weight, grade, opticalSize);

    /// <summary>
    /// Creates a Material Icon configuration with dynamic values, but allows for custom default values to be set.
    /// </summary>
    public static MdIconConfiguration CreateDynamic(bool defaultFill = false, int defaultWeight = 400,
        int defaultGrade = 0, int defaultOpticalSize = 24)
            => new(Dynamic, defaultFill, defaultWeight, 
                defaultGrade, defaultOpticalSize);

    /// <summary>
    /// Sets the default values (or the actual values when <see cref="IsStatic"/>).
    /// Set values here to null to leave the real values unchanged.
    /// </summary>
    public void SetValues(bool? fill, int? weight = null,
        int? grade = null, int? opticalSize = null)
    {
        Fill = fill ?? Fill;
        Weight = weight ?? Weight;
        Grade = grade ?? Grade;
        OpticalSize = opticalSize ?? OpticalSize;
    }

    // ReSharper disable StringLiteralTypo
    /// <summary>
    /// Creates a configuration string to append to a font file request.
    /// </summary>
    /// <returns></returns>
    public string GetFontConfigurationString() 
        => ":opsz,wght,FILL,GRAD@" // Optical Size, Weight, Fill, Grad
           + (IsStatic ? $"{OpticalSize},{Weight},{(Fill ? 1 : 0)},{Grade}" : "20..48,100..700,0..1,-50..200");
    // ReSharper restore StringLiteralTypo

    /// <summary>
    /// Creates a CSS font-variation-settings style for use on icon components to provide correct styling.
    /// </summary>
    public string GetFontVariationStyle()
        => $"font-variation-settings: 'FILL' {(Fill ? 1 : 0)}, 'wght' {Weight}, 'GRAD' {Grade}, 'opsz' {OpticalSize};";
}