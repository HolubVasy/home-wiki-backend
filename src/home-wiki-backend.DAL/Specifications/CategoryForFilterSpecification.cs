using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Models.Entities;
using home_wiki_backend.DAL.Extensions;
using home_wiki_backend.Shared.Models.Dtos;

/// <summary>
/// Specification for retrieving articles by category.
/// </summary>
public sealed class CategoryForFilterSpecification :
    SpecificationBase<Category>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleForFilterSpecification"/> class.
    /// </summary>
    /// <param name="pageFilterData">Data for filtered paging.</param>
    public CategoryForFilterSpecification(CategoryFilterRequestDto pageFilterData)
    {

        ApplySorting(pageFilterData.GetOrderBy());

        ApplyCriteria(pageFilterData.GetPredicate());
    }
}
