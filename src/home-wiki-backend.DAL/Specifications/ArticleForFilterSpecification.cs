﻿using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models.Dtos;

/// <summary>
/// Specification for retrieving articles by category.
/// </summary>
public sealed class ArticleForFilterSpecification :
    SpecificationBase<Article>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleForFilterSpecification"/> class.
    /// </summary>
    /// <param name="pageFilterData">Data for filtered paging.</param>
    public ArticleForFilterSpecification(ArticleFilterRequestDto pageFilterData)
    {
        // Include the related Category and Tags
        AddInclude(a => a.Category);
        AddInclude(a => a.Tags!);

        // Optionally, apply ordering
        ApplyOrderBy(a => a.Name);
    }
}
