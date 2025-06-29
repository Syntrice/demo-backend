namespace DemoBackend.Exceptions;

public class ServiceValidationException(string message) : Exception(message);