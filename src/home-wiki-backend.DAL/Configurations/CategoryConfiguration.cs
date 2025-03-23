using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using home_wiki_backend.DAL.Models.Entities;
using HomeWiki.DAL.Resources.Constants;
using home_wiki_backend.DAL.Configurations;

namespace HomeWiki.DAL.EntitiesConfiguration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TablesMetadata.Category.Name);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName(TablesMetadata.Category.PrimaryKeyName);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(StringMaxLengths.Long);
    }
}
