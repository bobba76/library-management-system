using System.Linq.Expressions;
using Library.Domain.CategoryAggregate;
using MediatR;

namespace Library.Application.CategoryAggregate.Queries;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Filter and OrderBy. Both are Case sensitivity
        Expression<Func<Category, bool>>? filter = null;
        Expression<Func<Category, string>>? orderBy = null;

        foreach (var property in typeof(Category).GetProperties())
        {
            // Find if any property name is matching with Filter input
            if (!string.IsNullOrWhiteSpace(request.FilterProperty) && request.FilterProperty.Equals(property.Name))
                filter = f => GetPropValue(f, property.Name).Equals(request.FilterValue);

            // Find if any property name is matching with OrderBy input
            if (!string.IsNullOrWhiteSpace(request.OrderBy) && request.OrderBy.Equals(property.Name))
                orderBy = o => GetPropValue(o, property.Name).ToString();
        }

        return await _categoryRepository.GetAsync(cancellationToken, filter, orderBy, request.OrderByDesc);
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}