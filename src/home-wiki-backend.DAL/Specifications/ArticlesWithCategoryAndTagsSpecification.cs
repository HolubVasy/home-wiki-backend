using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Models.Entities;


namespace home_wiki_backend.DAL.Specifications;

/// <summary>
/// Specification for retrieving articles with category and tags.
/// </summary>
public sealed class ArticlesWithCategoryAndTagsSpecification :
    SpecificationBase<Article>
{
    /// <summary>
    /// Initializes a new instance of the 
    /// <see cref="ArticlesByCategorySpecification"/> class.
    /// </summary>
    public ArticlesWithCategoryAndTagsSpecification()
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
