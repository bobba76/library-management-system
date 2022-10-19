using AutoMapper;
using Library.Domain.CategoryAggregate;
using Library.Domain.LibraryItemAggregate;
using Library.Domain.LibraryItemAggregate.Types;
using MediatR;

namespace Library.Application.LibraryItemAggregate.Commands;

public class UpdateLibraryItemCommandHandler : IRequestHandler<UpdateLibraryItemCommand, LibraryItem>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILibraryItemRepository _libraryItemRepository;
    private readonly IMapper _mapper;

    public UpdateLibraryItemCommandHandler(ILibraryItemRepository libraryItemRepository,
        ICategoryRepository categoryRepository, IMapper mapper)
    {
        _libraryItemRepository = libraryItemRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<LibraryItem> Handle(UpdateLibraryItemCommand request,
        CancellationToken cancellationToken)
    {
        var oldLibraryItem = await _libraryItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (request.CategoryId is not null)
        {
            var categories = await _categoryRepository.GetAsync(cancellationToken);

            var categoryExists = categories.Any(e => e.Id.Equals(request.CategoryId));

            if (categoryExists is false)
                throw new ArgumentException(
                    $"Category does not exist. (Parameter '{nameof(request.CategoryId)}, Value '{request.CategoryId}')");
        }

        if (request.BorrowDate is not null && request.Borrower is null ||
            request.BorrowDate is null && request.Borrower is not null)
            throw new ArgumentException(
                $"To borrow a book, both {nameof(request.Borrower)} and {nameof(request.BorrowDate)} must have a value. (Parameter '{nameof(request.Borrower)}, Value '{request.Borrower}', Parameter '{nameof(request.BorrowDate)}, Value '{request.BorrowDate}')");

        if (oldLibraryItem.BorrowDate is not null &&
            oldLibraryItem.Borrower is not null &&
            (
                request.Pages is not null ||
                request.RunTimeMinutes is not null ||
                request.CategoryId is not null ||
                request.Type is not null ||
                request.Author is not null ||
                request.Title is not null
            )
           )
            throw new ArgumentException(
                $"The Library Item cannot be modified while being borrowed. Please return the item before updating it. (Parameter '{nameof(oldLibraryItem.Borrower)}, Value '{oldLibraryItem.Borrower}', Parameter '{nameof(oldLibraryItem.BorrowDate)}, Value '{oldLibraryItem.BorrowDate}')");

        // Keeping Type if null
        request.Type ??= oldLibraryItem.Type;
        request.Author ??= oldLibraryItem.Author;
        request.Pages ??= oldLibraryItem.Pages;
        request.RunTimeMinutes ??= oldLibraryItem.RunTimeMinutes;

        var libraryItem = UpdateLibraryItemBasedOfType(_mapper.Map<UpdateLibraryItemParameters>(request));

        await _libraryItemRepository.UpdateAsync(libraryItem, cancellationToken);

        return await _libraryItemRepository.GetByIdAsync(request.Id, cancellationToken);
    }

    private static LibraryItem UpdateLibraryItemBasedOfType(UpdateLibraryItemParameters libraryItem)
    {
        switch (libraryItem.Type)
        {
            case LibraryItemType.AudioBook:
                return AudioBook.Update(libraryItem);

            case LibraryItemType.Dvd:
                return Dvd.Update(libraryItem);

            case LibraryItemType.ReferenceBook:
                return ReferenceBook.Update(libraryItem);

            case LibraryItemType.Book:
            default:
                return Book.Update(libraryItem);
        }
    }
}