namespace MaterialDesign.Icons.Common.Components;

public interface IDynamicComponentOutlet
{
    public static abstract void AddComponentSource<TComponent>(IDictionary<string, object>? attributes = null);
    
    public static abstract bool OutletExists();
}

public interface IDynamicIdComponentOutlet
{
    protected string Id { get; }
    
    public static abstract void AddComponentSourceWithId<TComponent>(string id, 
        IDictionary<string, object>? attributes = null);
    
    public static abstract bool OutletExistsWithId(string id);
}