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
            if (!string.IsNullOrWhiteSpace(request.FilterProperty) && request.FilterProperty.Equals(property.Name))
                filter = f => GetPropValue(f, property.Name).Equals(request.FilterValue);

            // Find if any property name is matching with OrderBy input
            if (!string.IsNullOrWhiteSpace(request.OrderBy) && request.OrderBy.Equals(property.Name))
                orderBy = o => GetPropValue(o, property.Name).ToString();
        }

        return await _libraryItemRepository.GetAsync(cancellationToken, filter, orderBy, request.OrderByDesc);
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}