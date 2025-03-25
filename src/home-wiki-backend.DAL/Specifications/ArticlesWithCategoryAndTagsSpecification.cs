using home_wiki_backend.DAL.Common.Models.Entities;

/// <summary>
/// Specification for retrieving articles with category and tags.
/// </summary>
public sealed class ArticlesWithCategoryAndTagsSpecification :
    SpecificationBase<Article>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArticlesByCategorySpecification"/> class.
    /// </summary>
    public ArticlesWithCategoryAndTagsSpecification()
    {
        // Include the related Category and Tags
        AddInclude(a => a.Category);
        AddInclude(a => a.Tags!);

        // Optionally, apply ordering
        ApplyOrderBy(a => a.Name);
    }
}
