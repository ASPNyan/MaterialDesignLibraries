using System.Diagnostics.CodeAnalysis;

namespace ExampleSite.Services;

public class StorageRequestResult
{
    public virtual required bool Success { get; init; }
    
    public Exception? Error { get; init; }

    public static StorageRequestResult FromError(Exception? error) => new()
    {
        Success = error is null,
        Error = error
    };
    
    public static StorageRequestResult<TOutput> FromOutput<TOutput>(TOutput? val) where TOutput : notnull => new()
    {
        Value = val,
        Success = val is not null
    };
}

public class StorageRequestResult<TOutput> : StorageRequestResult where TOutput : notnull
{
    [MemberNotNullWhen(true, nameof(Value))]
    public override required bool Success { get; init; }

    public required TOutput? Value { get; init; }
}