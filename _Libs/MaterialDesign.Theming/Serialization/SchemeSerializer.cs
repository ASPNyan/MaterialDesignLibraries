using System.Runtime.Serialization;
using System.Text.Json;
using MaterialDesign.Color.Schemes;

namespace MaterialDesign.Theming.Serialization;

/// <summary>
/// Provides functionality to serialize Schemes into strings and vice versa. Schemes are able to implement
/// custom serialization logic by implementing the <see cref="ISchemeSerializable{TSelf}"/> interface.
/// <br/><br/>
/// Schemes with the <see cref="ISchemeSerializable{TSelf}"/> can be serialized on instances and deserialized
/// with a static method. Additionally, the <see cref="Serialize{TScheme}">SchemeSerializer.Serialize(TScheme)</see>
/// and <see cref="Deserialize{TScheme}">SchemeSerializer.Deserialize(TScheme)</see> methods take a type parameter
/// limited to <see cref="ISchemeSerializable{TSelf}"/> which calls the same methods as mentioned before.
/// <br/><br/>
/// When schemes don't have <see cref="ISchemeSerializable{TSelf}"/> implemented,
/// <see cref="SerializeGeneric{TScheme}"/> and <see cref="DeserializeGeneric{TScheme}"/> can serialize and deserialize
/// generic <see cref="IScheme"/> instances by getting the <see cref="IScheme.Origin"/> and other important properties,
/// and use a smaller, private struct to serialize the information required. Deserialization with
/// <see cref="DeserializeGeneric{TScheme}"/> does require a factory method to be provided in order to construct the
/// scheme with a <see cref="SchemeGeneric"/>.
/// <br/><br/>
/// <see cref="DynamicScheme"/> inheriting records can also be serialized/deserialized with the
/// <see cref="SerializeDynamic{TScheme}"/> and <see cref="DeserializeDynamic{TScheme}"/> methods.
/// </summary>
public static class SchemeSerializer
{
    /// <summary>
    /// Serializes a <typeparamref name="TScheme"/> using the implemented
    /// <see cref="ISchemeSerializable{TSelf}.SerializeScheme"/> method.
    /// </summary>
    public static string Serialize<TScheme>(TScheme scheme) where TScheme : ISchemeSerializable<TScheme>, IScheme =>
        scheme.SerializeScheme();

    /// <summary>
    /// Deserializes a <typeparamref name="TScheme"/> using the implemented
    /// <see cref="ISchemeSerializable{TSelf}.DeserializeScheme"/> method.
    /// </summary>
    public static TScheme Deserialize<TScheme>(string serialized) where TScheme : ISchemeSerializable<TScheme>, IScheme =>
        TScheme.DeserializeScheme(serialized);
    
    /// <summary>
    /// Serializes an <see cref="IScheme"/> with its <see cref="IScheme.Origin"/>, <see cref="IScheme.IsDarkScheme"/>,
    /// and its <see cref="Type"/> as provided by <typeparamref name="TScheme"/>.
    /// </summary>
    public static string SerializeGeneric<TScheme>(TScheme scheme) where TScheme : IScheme =>
        JsonSerializer.Serialize(new SchemeGenericTyped(scheme.Origin, scheme.IsDarkScheme, 
            scheme.GetType().AssemblyQualifiedName));

    /// <summary>
    /// Deserializes a JSON string back into a scheme using the provided construction method. Type checking,
    /// using a <see cref="Type.AssemblyQualifiedName"/> can be disabled by setting the
    /// <paramref name="typeChecking"/> param to false, as it is true by default. Type checking will always
    /// fail if the provided <typeparamref name="TScheme"/> is <see cref="IScheme"/>.
    /// </summary>
    /// <returns>The <typeparamref name="TScheme"/>, constructed with the provided <paramref name="constructor"/></returns>
    public static TScheme DeserializeGeneric<TScheme>(string schemeSerialized,
        Func<SchemeGeneric, TScheme> constructor, bool typeChecking = true) where TScheme : IScheme
    {
        var generic = JsonSerializer.Deserialize<SchemeGenericTyped>(schemeSerialized);

        Type serializedType = GetType(generic.FullyQualifiedSchemeType);
        
        if (typeChecking && typeof(TScheme) != serializedType)
        {
            string storedClassName = serializedType.Name;
            string error = $"{storedClassName} cannot be converted to {typeof(TScheme).Name} with type checking enabled.";
            throw new SerializationException(error);
        }
        
        SchemeGeneric noType = new(generic.Origin, generic.IsDarkScheme);
        return constructor(noType);
    }

    public static IScheme DeserializeGeneric(string schemeSerialized, Func<SchemeGenericTyped, IScheme> constructor) => 
        constructor(JsonSerializer.Deserialize<SchemeGenericTyped>(schemeSerialized));

    /// <summary>
    /// Converts the provided <see cref="DynamicScheme"/> into a JSON string.
    /// </summary>
    public static string SerializeDynamic<TScheme>(TScheme scheme) where TScheme : DynamicScheme
    {
        DynamicSchemeJson schemeJson = new()
        {
            Source = scheme.Source,
            Variant = scheme.Variant,
            IsDarkScheme = scheme.IsDark,
            FullyQualifiedCustomType = scheme.GetType().AssemblyQualifiedName!
        };
        
        return JsonSerializer.Serialize(schemeJson);
    }

    /// <summary>
    /// Deserializes the provided JSON string into a <see cref="DynamicScheme"/> and converts it to the provided
    /// <typeparamref name="TScheme"/> with a safe <c>as</c> cast.
    /// </summary>
    public static TScheme DeserializeDynamic<TScheme>(string serialized)
        where TScheme : DynamicScheme
    {
        var schemeJson = JsonSerializer.Deserialize<DynamicSchemeJson>(serialized);

        return ((schemeJson.Variant is Variant.Custom
            ? DynamicScheme.Create(schemeJson.Source, GetType(schemeJson.FullyQualifiedCustomType), schemeJson.IsDarkScheme)
            : DynamicScheme.Create(schemeJson.Source, schemeJson.Variant, schemeJson.IsDarkScheme)) as TScheme)!;
    }
    
    private static Type GetType(string? type)
    {
        return type is null || type.Equals("null", StringComparison.InvariantCultureIgnoreCase)
            ? throw new TypeLoadException($"Failed to load type from type string of {type}. This may be " +
                                          $"due to a scheme from another assembly not being included.")
            : Type.GetType(type, true)!;
    }

    private record struct DynamicSchemeJson(HCTA Source, Variant Variant, bool IsDarkScheme, string FullyQualifiedCustomType);
    
    /// <summary>
    /// A miniature, readonly struct that contains an <see cref="Origin"/> <see cref="HCTA"/> color,
    /// a <see cref="bool"/> <see cref="IsDarkScheme"/>, and the <see cref="Type"/>
    /// <see cref="Type.AssemblyQualifiedName"/> stored in <see cref="FullyQualifiedSchemeType"/>
    /// that can all be used to construct an <see cref="IScheme"/>.
    /// </summary>
    /// <param name="Origin">
    /// The <see cref="IScheme.Origin"/> property of the scheme, null when <see cref="IScheme.Origin"/> is null
    /// or when it is unavailable to get.
    /// </param>
    public readonly record struct SchemeGenericTyped(HCTA? Origin, bool IsDarkScheme, string? FullyQualifiedSchemeType);
    
    /// <summary>
    /// A miniature, readonly struct that contains an <see cref="Origin"/> <see cref="HCTA"/> color and
    /// a <see cref="bool"/> <see cref="IsDarkScheme"/> that can be used together to construct an <see cref="IScheme"/>.
    /// </summary>
    /// <param name="Origin">
    /// The <see cref="IScheme.Origin"/> property of the scheme, null when <see cref="IScheme.Origin"/> is null
    /// or when it is unavailable to get.
    /// </param>
    public readonly record struct SchemeGeneric(HCTA? Origin, bool IsDarkScheme);
}
