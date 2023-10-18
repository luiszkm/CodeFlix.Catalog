

using CodeFlix.Catalog.Domain.Domain.SeedWork;
using CodeFlix.Catalog.Domain.Domain.Validation;
using System.Collections.Generic;

namespace CodeFlix.Catalog.Domain.Domain.Entity;
public class Genre : AggregateRoot
{
    public Genre(
        string name, bool isActive = true)
    {
        Name = name;
        _categories = new List<Guid>();
        IsActive = isActive;
        Validate();
    }
    private List<Guid> _categories;
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<Guid> Categories =>
        (IReadOnlyList<Guid>)_categories.AsReadOnly();


    public void Update(string name)
    {
        Name = name;
        Validate();
    }
    public void Activate()
    {
        IsActive = true;
        Validate();
    }
    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }
    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
    }

    public void AddCategory(Guid categoryId)
    {
        _categories.Add(categoryId);
        Validate();
    }

    public void RemoveCategory(Guid categoryId)
    {
        _categories.Remove(categoryId);
        Validate();
    }

    public void RemoveAllCategories()
    {
        _categories.Clear();
        Validate();
    }
}
