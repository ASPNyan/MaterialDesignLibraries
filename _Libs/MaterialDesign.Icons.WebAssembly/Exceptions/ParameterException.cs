namespace MaterialDesign.Icons.Exceptions;

public class ParameterException(string message, string paramName, Exception? innerException = null)
    : ArgumentException(message, paramName, innerException);