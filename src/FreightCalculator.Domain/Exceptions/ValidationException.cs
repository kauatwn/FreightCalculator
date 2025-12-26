namespace FreightCalculator.Domain.Exceptions;

public sealed class ValidationException(string message) : Exception(message);