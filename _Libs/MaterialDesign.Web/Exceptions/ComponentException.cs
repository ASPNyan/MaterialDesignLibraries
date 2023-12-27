namespace MaterialDesign.Web.Exceptions;

public class ComponentException(string? message = null, Exception? innerException = null) 
    : Exception(message, innerException);