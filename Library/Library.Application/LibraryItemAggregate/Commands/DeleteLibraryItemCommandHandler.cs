using Library.Domain.LibraryItemAggregate;
using MediatR;

namespace Library.Application.LibraryItemAggregate.Commands;

public class DeleteLibraryItemCommandHandler : IRequestHandler<DeleteLibraryItemCommand, IEnumerable<LibraryItem>>
{
    private readonly ILibraryItemRepository _libraryItemRepository;

    public DeleteLibraryItemCommandHandler(ILibraryItemRepository libraryItemRepository)
    {
        _libraryItemRepository = libraryItemRepository;
    }

    public async Task<IEnumerable<LibraryItem>> Handle(DeleteLibraryItemCommand request,
        CancellationToken cancellationToken)
    {
        var libraryItem = await _libraryItemRepository.GetByIdAsync(request.Id, cancellationToken);

        await _libraryItemRepository.DeleteAsync(libraryItem, cancellationToken);

        return await _libraryItemRepository.GetAsync(cancellationToken);
    }
}