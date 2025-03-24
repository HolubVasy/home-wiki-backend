using home_wiki_backend.DAL.Common.Models.Entities;

public class ArticlesByCategorySpecification : 
    SpecificationBase<Article>
{
    public ArticlesByCategorySpecification(int categoryId)
        : base(a => a.CategoryId == categoryId)
    {
        // Include the related Category and Tags
        AddInclude(a => a.Category);
        AddInclude(a => a.Tags);

        // Optionally, apply ordering
        ApplyOrderBy(a => a.Name);
    }
}
