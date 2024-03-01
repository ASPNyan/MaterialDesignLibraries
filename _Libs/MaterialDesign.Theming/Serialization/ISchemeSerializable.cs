namespace MaterialDesign.Theming.Serialization;

public interface ISchemeSerializable<out TSelf> where TSelf : ISchemeSerializable<TSelf>, IScheme
{
    public string SerializeScheme();
    public static abstract TSelf DeserializeScheme(string serialized);
}