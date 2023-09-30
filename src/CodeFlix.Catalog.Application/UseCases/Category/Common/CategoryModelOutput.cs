using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Application.UseCases.Category.Common;
public class CategoryModelOutput
{
    public CategoryModelOutput(
        Guid id,
        string name,
        string description,
        bool isActive,
         DateTime createdAt
        )
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
    }
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public static CategoryModelOutput FromCategory(DomainEntity.Category category)
    {
        return new CategoryModelOutput(
           category.Id,
           category.Name,
           category.Description,
           category.IsActive,
           category.CreatedAt
           );
    }
}

