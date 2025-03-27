using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.Shared.Models.Dtos;

namespace home_wiki_backend.DAL.Specifications;

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
        AddInclude(article => article.Category);
        AddInclude(article => article.Tags!);

        // Optionally, apply ordering
        ApplySorting(article => article.OrderBy(a => a.Name));
    }
}
