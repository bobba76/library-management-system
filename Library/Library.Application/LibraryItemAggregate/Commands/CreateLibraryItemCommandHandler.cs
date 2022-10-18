using AutoMapper;
using Library.Domain.CategoryAggregate;
using Library.Domain.LibraryItemAggregate;
using Library.Domain.LibraryItemAggregate.Types;
using MediatR;

namespace Library.Application.LibraryItemAggregate.Commands;

public class CreateLibraryItemCommandHandler : IRequestHandler<CreateLibraryItemCommand, IEnumerable<LibraryItem>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILibraryItemRepository _libraryItemRepository;
    private readonly IMapper _mapper;

    public CreateLibraryItemCommandHandler(ILibraryItemRepository libraryItemRepository,
        ICategoryRepository categoryRepository, IMapper mapper)
    {
        _libraryItemRepository = libraryItemRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LibraryItem>> Handle(CreateLibraryItemCommand request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAsync(cancellationToken);

        var categoryExists = categories.Any(c => c.Id.Equals(request.CategoryId));

        if (categoryExists is false)
            throw new ArgumentException(
                $"Category does not exist. (Parameter '{nameof(request.CategoryId)}, Value '{request.CategoryId}')");

        var libraryItem = CreateLibraryItemBasedOfType(_mapper.Map<CreateLibraryItemParameters>(request));

        await _libraryItemRepository.CreateAsync(libraryItem, cancellationToken);

        return await _libraryItemRepository.GetAsync(cancellationToken);
    }

    private static LibraryItem CreateLibraryItemBasedOfType(CreateLibraryItemParameters libraryItem)
    {
        switch (libraryItem.Type)
        {
            case LibraryItemType.AudioBook:
                return AudioBook.Create(libraryItem);

            case LibraryItemType.Dvd:
                return Dvd.Create(libraryItem);

            case LibraryItemType.ReferenceBook:
                return ReferenceBook.Create(libraryItem);

            case LibraryItemType.Book:
            default:
                return Book.Create(libraryItem);
        }
    }
}