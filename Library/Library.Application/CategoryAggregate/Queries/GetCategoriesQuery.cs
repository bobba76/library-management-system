using Library.Domain.CategoryAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.CategoryAggregate.Queries;

public class GetCategoriesQuery : IQuery<IEnumerable<Category>>
{
    // The queries are case-sensitive
    // CategoryName
    public string? OrderBy { get; set; } = null;

    public bool? OrderByDesc { get; set; } = false;

    // Id, CategoryName ...
    public string? FilterProperty { get; set; }

    // 5, Romance ...
    public string? FilterValue { get; set; }
}