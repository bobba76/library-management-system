using Library.Domain.CategoryAggregate;
using MediatR;

namespace Library.Application.CategoryAggregate.Queries;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Category>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}