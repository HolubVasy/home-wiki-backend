using home_wiki_backend.DAL.Common.Models.Entities;

/// <summary>
/// Specification for retrieving articles by category.
/// </summary>
public sealed class ArticleForFilterSpecification :
    SpecificationBase<Article>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleForFilterSpecification"/> class.
    /// </summary>
    /// <param name="categoryId">The ID of the category to filter articles by.</param>
    public ArticleForFilterSpecification(int categoryId)
        : base(a => a.CategoryId == categoryId)
    {
        // Include the related Category and Tags
        AddInclude(a => a.Category);
        AddInclude(a => a.Tags!);

        // Optionally, apply ordering
        ApplyOrderBy(a => a.Name);
    }
}
