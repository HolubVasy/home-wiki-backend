using Microsoft.EntityFrameworkCore;
using home_wiki_backend.DAL.Models.Entities;
using HomeWiki.DAL.EntitiesConfiguration;

namespace home_wiki_backend.DAL.Data;

public class DbWikiContext : DbContext
{
    public DbWikiContext(DbContextOptions<DbWikiContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
    }
}
