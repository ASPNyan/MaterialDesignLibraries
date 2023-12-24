namespace MaterialDesign.Theming.Web.Generic;

public class BoundValue<TValue>(TValue value)
{
    public TValue Get() => value;
    public void Set(TValue newValue)
    {
        value = newValue;
        OnUpdate?.Invoke();
    }

    public static implicit operator TValue(BoundValue<TValue> value) => value.Get();

    public event Action? OnUpdate;
}