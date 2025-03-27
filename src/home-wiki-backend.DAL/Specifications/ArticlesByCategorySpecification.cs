using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Models.Entities;

namespace home_wiki_backend.DAL.Specifications;

/// <summary>
/// Specification for retrieving articles by category.
/// </summary>
public sealed class ArticlesByCategorySpecification :
    SpecificationBase<Article>
{
    /// <summary>
    /// Initializes a new instance of the <see cref=
    /// "ArticlesByCategorySpecification"/> class.
    /// </summary>
    /// <param name="categoryId">The ID of the category 
    /// to filter articles by.</param>
    public ArticlesByCategorySpecification(int categoryId)
        : base(a => a.CategoryId == categoryId)
    {
        // Include the related Category and Tags
        AddInclude(article => article.Category);
        AddInclude(article => article.Tags!);

        // Optionally, apply ordering
        ApplySorting(
            article => article
                .OrderBy(a => a.Name));
    }
}
