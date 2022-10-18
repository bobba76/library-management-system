using Library.Domain.CategoryAggregate;
using Library.Domain.LibraryItemAggregate;
using MediatR;

namespace Library.Application.CategoryAggregate.Commands;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IEnumerable<Category>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILibraryItemRepository _libraryItemRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, ILibraryItemRepository libraryItemRepository)
    {
        _categoryRepository = categoryRepository;
        _libraryItemRepository = libraryItemRepository;
    }

    public async Task<IEnumerable<Category>> Handle(DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

        var libraryItems = await _libraryItemRepository.GetAsync(cancellationToken);

        var referencesToLibraryItems = libraryItems.Count(l =>
            l.CategoryId.Equals(request.Id));

        if (referencesToLibraryItems > 0)
            throw new ArgumentException(
                $"Category cannot be deleted. The category is referenced in {referencesToLibraryItems} library items. Remove references before deleting category.");

        await _categoryRepository.DeleteAsync(category, cancellationToken);

        return await _categoryRepository.GetAsync(cancellationToken);
    }
}