using ExampleSite.Components.Settings;
using ExampleSite.DefaultThemes;
using MaterialDesign.Color.Schemes.Custom;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Serialization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ExampleSite.Services;

public sealed class SettingsStorageService(ProtectedLocalStorage localStorage, ILogger<SettingsStorageService> logger)
{
    private ProtectedLocalStorage BrowserStorage { get; } = localStorage;
    public ILogger<SettingsStorageService> Logger { get; } = logger;

    private const string Purpose = "Settings";
    
    private const string SchemeStorage = nameof(SchemeStorage);
    private const string ExtraSchemeSettings = nameof(ExtraSchemeSettings);
    
    public async ValueTask<StorageRequestResult<IScheme>> GetSchemeAsync(IScheme? fallback = null)
    {
        Exception? error = null;
        IScheme? output = null;
        try
        {
            var serializedResult = await BrowserStorage.GetAsync<string>(Purpose, SchemeStorage);
            var customSchemeResult =
                await BrowserStorage.GetAsync<ModifiableCustomScheme>(Purpose, ExtraSchemeSettings);

            if (!serializedResult.Success || serializedResult.Value is null)
                return StorageRequestResult.FromOutput<IScheme>(null);

            output = SchemeSerializer.DeserializeGeneric(serializedResult.Value, typed =>
            {
                if (string.IsNullOrWhiteSpace(typed.FullyQualifiedSchemeType)) return fallback
                    ?? throw new ArgumentException("The type was not provided in the provided serialization.\nValue: " +
                                                          serializedResult.Value);

                Type? type = Type.GetType(typed.FullyQualifiedSchemeType);
                
                if (type is null)
                    return fallback ?? throw new TypeLoadException(
                        $"Unable to get type {typed.FullyQualifiedSchemeType}. Are the required assemblies added?");
                
                if (type.BaseType == typeof(DefaultThemeBase))
                {
                    if (type == typeof(Oceanic)) return new Oceanic();
                    if (type == typeof(Moonlight)) return new Moonlight();
                    if (type == typeof(Volcano)) return new Volcano();
                    if (type == typeof(Plasma)) return new Plasma();

                    return (IScheme)Activator.CreateInstance(type)!;
                }

                if (type == typeof(Theme))
                {
                    if (typed.Origin is null)
                        throw new ArgumentException(
                            "The color of the scheme could not be found in the provided serialization.");

                    return new Theme(typed.Origin);
                }

                if (type == typeof(ModifiableCustomScheme))
                {
                    if (!customSchemeResult.Success || customSchemeResult.Value is null)
                        throw new ArgumentException(
                            "Stored scheme was ModifiableCustomScheme, but ModifiableCustomScheme options could be found.");
                    return customSchemeResult.Value;
                }

                return fallback ?? throw new Exception($"Could not construct type {type.FullName}");
            });
        }
        catch (Exception e)
        {
            error = e;
        }
        
        StorageRequestResult<IScheme> result = new()
        {
            Value = output,
            Error = error,
            Success = output is not null && error is null
        };
        return result;
    }

    public async ValueTask<StorageRequestResult> SetSchemeAsync<TScheme>(TScheme scheme) where TScheme : IScheme
    {
        if (typeof(TScheme) == typeof(ModifiableCustomScheme)) 
            await BrowserStorage.SetAsync(Purpose, ExtraSchemeSettings, scheme);
        
        string serialized = SchemeSerializer.SerializeGeneric(scheme); // todo: AssemblyQualifiedName that is found in here is null?
        return await TryAndReturnFromError(BrowserStorage.SetAsync(Purpose, SchemeStorage, serialized));
    }

    public async ValueTask<StorageRequestResult> DeleteSchemeAsync() =>
        await TryAndReturnFromError(BrowserStorage.DeleteAsync(SchemeStorage));

    private const string LayoutSettingsStorage = nameof(LayoutSettingsStorage);

    public async ValueTask<StorageRequestResult<LayoutSettings>> GetLayoutSettingsAsync(LayoutSettings? fallback = null)
    {
        var result = await BrowserStorage.GetAsync<LayoutSettings?>(Purpose, LayoutSettingsStorage);
        return StorageRequestResult.FromOutput(result.Success ? result.Value ?? fallback : fallback);
    }

    public async ValueTask<StorageRequestResult> SetLayoutSettingsAsync(LayoutSettings layoutSettings) =>
        await TryAndReturnFromError(BrowserStorage.SetAsync(LayoutSettingsStorage, layoutSettings));

    public async ValueTask<StorageRequestResult> DeleteLayoutSettingsAsync() =>
        await TryAndReturnFromError(BrowserStorage.DeleteAsync(LayoutSettingsStorage));

    private static async ValueTask<StorageRequestResult> TryAndReturnFromError(ValueTask task)
    {
        Exception? error = null;

        try
        {
            await task;
        }
        catch (Exception e)
        {
            error = e;
        }

        return StorageRequestResult.FromError(error);
    }
    
    
}