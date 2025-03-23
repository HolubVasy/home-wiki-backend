using Microsoft.EntityFrameworkCore;
using home_wiki_backend.DAL.Models.Entities;
using HomeWiki.DAL.EntitiesConfiguration;

namespace home_wiki_backend.DAL.Data;

public class DbWikiContext : DbContext
{
    public DbWikiContext(DbContextOptions<DbWikiContext> options) :
        base(options)
    {
        Options = options;
    }

    public DbContextOptions<DbWikiContext> Options { get; }

    #region Assign entities

    public DbSet<Article> Articles { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;

    #endregion

    #region Configure entities

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
    }

    #endregion
}
