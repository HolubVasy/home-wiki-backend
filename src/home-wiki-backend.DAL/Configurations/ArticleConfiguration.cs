using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HomeWiki.DAL.Resources.Constants;
using home_wiki_backend.DAL.Configurations;
using home_wiki_backend.DAL.Common.Models.Entities;

namespace HomeWiki.DAL.EntitiesConfiguration;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable(TablesMetadata.Article.Name);

        builder.HasKey(x => x.Id);

        #region Columns

        builder.Property(x => x.Id)
            .HasColumnName(TablesMetadata.Article.PrimaryKeyName);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Long);
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Long);
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Huge);
        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Short50);
        builder.Property(x => x.ModifiedBy)
            .HasMaxLength(StringMaxLengths.Short50);

        #endregion

        #region Relations

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Articles!)
            .HasForeignKey(nameof(Article.CategoryId))
            .HasConstraintName(TablesMetadata.Article.ForeignKeyToCategory)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity(j => j.ToTable(
                TablesMetadata.ArticleTag.JoinTableName));

        #endregion
    }
}
