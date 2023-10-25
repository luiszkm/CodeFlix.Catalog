

using CodeFlix.Catalog.Domain.Domain.Entity;
using CodeFlix.Catalog.Infra.Data.EF.Configurations;
using CodeFlix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Infra.Data.EF;
public class CodeflixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<GenreCategories> GenreCategories => Set<GenreCategories>();

    public CodeflixCatalogDbContext(DbContextOptions<CodeflixCatalogDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());

        modelBuilder.ApplyConfiguration(new GenresCategoryConfiguration());

    }
}
