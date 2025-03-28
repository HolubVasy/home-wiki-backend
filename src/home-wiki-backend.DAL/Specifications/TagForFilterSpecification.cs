using home_wiki_backend.BL.Extensions;
using home_wiki_backend.BL.Models;
using home_wiki_backend.DAL.Common.Contracts.Specifications;
using home_wiki_backend.DAL.Common.Models.Entities;

/// <summary>
/// Specification for retrieving articles by category.
/// </summary>
public sealed class TagForFilterSpecification :
    SpecificationBase<Tag>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleForFilterSpecification"/> class.
    /// </summary>
    /// <param name="pageFilterData">Data for filtered paging.</param>
    public TagForFilterSpecification(TagFilterRequestDto pageFilterData)
    {

        ApplySorting(pageFilterData.GetOrderBy());

        ApplyCriteria(pageFilterData.GetPredicate());
    }
}
