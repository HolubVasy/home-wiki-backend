﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.DAL.Common.Resources;

namespace home_wiki_backend.DAL.Common.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TablesMetadata.Category.Name);

        builder.HasKey(x => x.Id);

        #region Indexes

        builder.HasIndex(x => x.Name)
            .IsUnique();

        #endregion

        #region Columns

        builder.Property(x => x.Id)
            .HasColumnName(TablesMetadata.Category.PrimaryKeyName);
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
