using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HomeWiki.DAL.Resources.Constants;
using home_wiki_backend.DAL.Configurations;
using home_wiki_backend.DAL.Common.Models.Entities;

namespace HomeWiki.DAL.EntitiesConfiguration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable(TablesMetadata.Tag.Name);

        builder.HasKey(x => x.Id);

        #region Indexes

        builder.HasIndex(x => x.Name)
            .IsUnique();

        #endregion

        #region Columns

        builder.Property(x => x.Id)
            .HasColumnName(TablesMetadata.Tag.PrimaryKeyName);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Long);
        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Short50);
        builder.Property(x => x.ModifiedBy)
            .HasMaxLength(StringMaxLengths.Short50);

        #endregion
    }
}
