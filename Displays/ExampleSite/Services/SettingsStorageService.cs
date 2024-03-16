using MaterialDesign.Theming;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ExampleSite.Services;

public sealed class SettingsStorageService(ProtectedLocalStorage browserStorage)
{
    public ProtectedLocalStorage BrowserStorage { get; } = browserStorage;

    private const string Purpose = "Settings";
    
    private const string SchemeStorage = nameof(SchemeStorage);
    
    public async ValueTask<IScheme> GetSchemeAsync(IScheme fallback)
    {
        var result = await BrowserStorage.GetAsync<IScheme>(Purpose, SchemeStorage);
        return !result.Success ? fallback : result.Value ?? fallback;
    }

    public async ValueTask<IScheme?> GetSchemeAsync() => await GetSchemeAsync(null!);
}