using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using home_wiki_backend.DAL.Models.Entities;
using HomeWiki.DAL.Resources.Constants;
using home_wiki_backend.DAL.Configurations;

namespace HomeWiki.DAL.EntitiesConfiguration;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable(TablesMetadata.Article.Name);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(TablesMetadata.Article.PrimaryKeyName);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Long);

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Articles!)
            .HasForeignKey(nameof(Article.CategoryId))
            .HasConstraintName(TablesMetadata.Article.ForeignKeyToCategory)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity(j => j.ToTable(
                TablesMetadata.ArticleTag.JoinTableName));
    }
}
