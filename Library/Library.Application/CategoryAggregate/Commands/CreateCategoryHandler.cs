using AutoMapper;
using Library.Domain.CategoryAggregate;
using MediatR;

namespace Library.Application.CategoryAggregate.Commands;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateCategoryCommand, IEnumerable<Category>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Category>> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAsync(cancellationToken);

        var categoryExists = categories.Any(e => e.CategoryName.Equals(request.CategoryName));

        if (categoryExists)
            throw new ArgumentException(
                $"No duplicates. A Category with that name already exists. (Parameter '{nameof(request.CategoryName)}, Value '{request.CategoryName}')");

        var category = Category.Create(_mapper.Map<CreateCategoryParameters>(request));

        await _categoryRepository.CreateAsync(category, cancellationToken);

        return await _categoryRepository.GetAsync(cancellationToken);
    }
}