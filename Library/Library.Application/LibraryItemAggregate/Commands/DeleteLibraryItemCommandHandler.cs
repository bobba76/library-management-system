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

        if (libraryItem.BorrowDate is not null &&
            libraryItem.Borrower is not null
           )
            throw new ArgumentException(
                $"The Library Item cannot be deleted while being borrowed. Please return the item before deleting it. (Parameter '{nameof(libraryItem.Borrower)}, Value '{libraryItem.Borrower}', Parameter '{nameof(libraryItem.BorrowDate)}, Value '{libraryItem.BorrowDate}')");


        await _libraryItemRepository.DeleteAsync(libraryItem, cancellationToken);

        return await _libraryItemRepository.GetAsync(cancellationToken);
    }
}