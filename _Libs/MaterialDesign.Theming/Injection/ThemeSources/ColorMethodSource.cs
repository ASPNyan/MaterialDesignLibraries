namespace MaterialDesign.Theming.Injection.ThemeSources;

/// <summary>
/// The <c>With</c> methods supplied by implementations of the ColorMethodSource can be subscribed to events for
/// an easier time updating them, but just calling the methods themselves and passing them around as
/// <see cref="Action">Actions</see> can be viable as well, but event subscriptions are simpler.
/// </summary>
public class ColorMethodSource : IThemeSource
{
    private Task<HCTA>? SourceTask { get; set; }
    private Func<Task<HCTA>>? SourceTaskMethod { get; set; }

    public void WithSource(HCTA source)
    {
        SourceTask = new Task<HCTA>(() => source);
        SourceTaskMethod = null;
    }

    public void WithTask(Task<HCTA> sourceTask)
    {
        SourceTask = sourceTask;
        SourceTaskMethod = null;
    }

    public void WithTaskMethod(Func<Task<HCTA>> sourceTaskMethod)
    {
        SourceTask = null;
        SourceTaskMethod = sourceTaskMethod;
    }

    public void WithMethod(Func<HCTA> sourceMethod)
    {
        SourceTask = new Task<HCTA>(sourceMethod);
        SourceTaskMethod = null;
    }

    public bool CheckSourceGetter(bool throwIfInvalid)
    {
        if (throwIfInvalid) if (SourceTask is null && SourceTaskMethod is null) throw new ArgumentNullException(null,
            $"{nameof(ColorMethodSource)} requires that the source be set with one of its `With` methods.");
        return SourceTask is null && SourceTaskMethod is null;
    }
    
    public Task<HCTA> GetSource()
    {
        CheckSourceGetter(true);
        return SourceTask ?? SourceTaskMethod!(); /* not null because the first check passing means one is not null, and if SourceTask
                                                   * is null, then SourceTaskMethod cannot be null. */
    }
}