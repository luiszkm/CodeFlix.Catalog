using CodeFlix.Catalog.Domain.Domain.Exceptions;
namespace CodeFlix.Catalog.Domain.Domain.Entity;
public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool  IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    

    public Category(string name, string description, bool isActive = true  )

    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Validate()
    {
        if (String.IsNullOrWhiteSpace(Name))
        {
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
        }
    }
}
