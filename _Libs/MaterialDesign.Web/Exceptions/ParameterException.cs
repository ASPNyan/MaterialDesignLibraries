namespace MaterialDesign.Web.Exceptions;

/// <summary>
/// The exception that is thrown when one of the parameters provided to a component is not valid.
/// </summary>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="paramName">The name of the parameter that caused the current exception.</param>
/// <param name="innerException">
/// The exception that is the cause of the current exception.
/// If the innerException parameter is not a null reference, the current exception is raised in a
/// catch block that handles the inner exception.
/// </param>
public class ParameterException(string message, string paramName, Exception? innerException = null)
    : ArgumentException(message, paramName, innerException);