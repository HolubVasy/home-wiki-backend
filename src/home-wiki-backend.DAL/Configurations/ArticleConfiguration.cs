using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.DAL.Common.Resources;

namespace home_wiki_backend.DAL.Configurations;

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
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Huge);
        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Short50);
        builder.Property(x => x.ModifiedBy)
            .HasMaxLength(StringMaxLengths.Short50);

        #endregion

        #region Indexes

        builder.HasIndex(x => x.Name)
            .IsUnique();

        #endregion

        #region Relations

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Articles!)
            .HasForeignKey(nameof(Article.CategoryId))
            .HasConstraintName(TablesMetadata.Article.ForeignKeyToCategory)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity<Dictionary<string, object>>(
                TablesMetadata.ArticleTag.JoinTableName,
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName(TablesMetadata.ArticleTag.ForeignKeyTag)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Article>()
                    .WithMany()
                    .HasForeignKey("ArticleId")
                    .HasConstraintName(TablesMetadata.ArticleTag.ForeignKeyArticle)
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ArticleId", "TagId");
                    j.ToTable(TablesMetadata.ArticleTag.JoinTableName);
                });

        #endregion
    }
}
