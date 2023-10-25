

using CodeFlix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFlix.Catalog.Infra.Data.EF.Configurations;
internal class GenresCategoryConfiguration : IEntityTypeConfiguration<GenreCategories>
{
    public void Configure(EntityTypeBuilder<GenreCategories> builder)
    {
        builder.HasKey(relation =>
            new { relation.CategoryId, relation.GenreId });

    }
}
