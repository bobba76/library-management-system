using Library.Domain.LibraryItemAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.LibraryItemAggregate.Queries;

public class GetLibraryItemsQuery : IQuery<IEnumerable<LibraryItem>>
{
    // The queries are case-sensitive
    // Title
    public string? OrderBy { get; set; } = null;

    public bool? OrderByDesc { get; set; } = false;

    // Title, Author ...
    public string? FilterProperty { get; set; }

    // The Witcher, IT ...
    public string? FilterValue { get; set; }
}