namespace CodeFlix.Catalog.Domain.Domain.Exceptions;
public class EntityValidationException : Exception
{
    public EntityValidationException(string? message) : base(message)
    {
    }
}
