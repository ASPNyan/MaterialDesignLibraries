using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Exceptions;

/// <summary>
/// The exception that is thrown when a component that is provided child content does not support it, like image elements.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">
/// The exception that is the cause of the current exception.
/// If the innerException parameter is not a null reference, the current exception is raised in a catch block
/// that handles the inner exception.</param>
public class VoidElementException(string? message = null, Exception? innerException = null)
    : ComponentException(message, innerException)
{
    public static VoidElementException ChildContentIsDisallowed<T>() where T : ComponentBase 
        => new($"Component `{typeof(T).FullName}` is a Void Element, and therefore cannot have child content.");
}