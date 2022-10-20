using System.Linq.Expressions;
using Library.Domain.LibraryItemAggregate;
using MediatR;

namespace Library.Application.LibraryItemAggregate.Queries;

public class GetLibraryItemsQueryHandler : IRequestHandler<GetLibraryItemsQuery, IEnumerable<LibraryItem>>
{
    private readonly ILibraryItemRepository _libraryItemRepository;

    public GetLibraryItemsQueryHandler(ILibraryItemRepository libraryItemRepository)
    {
        _libraryItemRepository = libraryItemRepository;
    }

    public async Task<IEnumerable<LibraryItem>> Handle(GetLibraryItemsQuery request,
        CancellationToken cancellationToken)
    {
        // Filter and OrderBy. Both are Case sensitivity
        Expression<Func<LibraryItem, bool>>? filter = null;
        Expression<Func<LibraryItem, string>>? orderBy = null;

        foreach (var property in typeof(LibraryItem).GetProperties())
        {
            // Find if any property name is matching with Filter input
            if (request.FilterProperty is not null && string.Equals(request.FilterProperty, property.Name,
                    StringComparison.OrdinalIgnoreCase))
                filter = f => GetPropValue(f, property.Name).Equals(request.FilterValue);

            // Find if any property name is matching with OrderBy input
            if (request.OrderBy is not null &&
                string.Equals(request.OrderBy, property.Name, StringComparison.OrdinalIgnoreCase))
                orderBy = o => GetPropValue(o, property.Name).ToString();
        }

        return await _libraryItemRepository.GetAsync(cancellationToken, filter, orderBy, request.OrderByDesc);
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}