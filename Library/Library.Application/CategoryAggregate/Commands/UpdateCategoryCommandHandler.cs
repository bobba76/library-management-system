using AutoMapper;
using Library.Domain.CategoryAggregate;
using MediatR;

namespace Library.Application.CategoryAggregate.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Category> Handle(UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAsync(cancellationToken);

        var categoryExists = categories.Any(e =>
            string.Equals(e.CategoryName, request.CategoryName, StringComparison.OrdinalIgnoreCase));


        if (categoryExists)
            throw new ArgumentException(
                $"No duplicates. A Category with that name already exists. (Parameter '{nameof(request.CategoryName)}, Value '{request.CategoryName}')");


        var category = Category.Update(_mapper.Map<UpdateCategoryParameters>(request));

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}