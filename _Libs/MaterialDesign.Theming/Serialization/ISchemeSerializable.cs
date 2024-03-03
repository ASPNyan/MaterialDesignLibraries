namespace MaterialDesign.Theming.Serialization;

/// <summary>
/// Provides basic serialization methods to Color <see cref="IScheme">Schemes</see>.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface ISchemeSerializable<out TSelf> where TSelf : ISchemeSerializable<TSelf>, IScheme
{
    /// <summary>
    /// Serializes a scheme into a string that can be deserialized with <see cref="DeserializeScheme"/>
    /// or with <see cref="SchemeSerializer.Deserialize{TScheme}"/>.
    /// </summary>
    public string SerializeScheme();
    
    /// <summary>
    /// Deserializes a string that was serialized with <see cref="SerializeScheme"/> or
    /// <see cref="SchemeSerializer.Serialize{TScheme}"/>.
    /// </summary>
    public static abstract TSelf DeserializeScheme(string serialized);
}