namespace MaterialDesign.Web.Exceptions;

/// <summary>
/// The exception that is thrown when a way that a component is used is not valid.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="innerException">
/// The exception that is the cause of the current exception.
/// If the innerException parameter is not a null reference, the current exception is raised in a catch block
/// that handles the inner exception.
/// </param>
public class ComponentException(string? message = null, Exception? innerException = null) 
    : Exception(message, innerException);