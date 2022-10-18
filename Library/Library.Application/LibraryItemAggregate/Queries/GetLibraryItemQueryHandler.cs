using Library.Domain.LibraryItemAggregate;
using MediatR;

namespace Library.Application.LibraryItemAggregate.Queries;

public class GetLibraryItemQueryHandler : IRequestHandler<GetLibraryItemQuery, LibraryItem>
{
    private readonly ILibraryItemRepository _libraryItemRepository;

    public GetLibraryItemQueryHandler(ILibraryItemRepository libraryItemRepository)
    {
        _libraryItemRepository = libraryItemRepository;
    }

    public async Task<LibraryItem> Handle(GetLibraryItemQuery request, CancellationToken cancellationToken)
    {
        return await _libraryItemRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}