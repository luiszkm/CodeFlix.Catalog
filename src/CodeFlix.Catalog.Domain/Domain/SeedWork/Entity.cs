

namespace CodeFlix.Catalog.Domain.Domain.SeedWork;
public abstract class Entity
{
    public Guid Id { get; private set; }

    protected Entity() => Id = Guid.NewGuid();
}
