using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Exceptions;

public class VoidElementException(string? message = null, Exception? innerException = null)
    : ComponentException(message, innerException)
{
    public static VoidElementException ChildContentIsDisallowed<T>() where T : ComponentBase 
        => new($"Component `{typeof(T).FullName}` is a Void Element, and therefore cannot have child content.");
}