using System.Linq.Expressions;
using Library.Domain.EmployeeAggregate;
using MediatR;

namespace Library.Application.EmployeeAggregate.Queries;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeBase>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeBase>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        // Filter and OrderBy. Both are Case sensitivity
        Expression<Func<EmployeeBase, bool>>? filter = null;
        Expression<Func<EmployeeBase, string>>? orderBy = null;

        foreach (var property in typeof(EmployeeBase).GetProperties())
        {
            // Find if any property name is matching with Filter input
            if (request.FilterProperty is not null && string.Equals(request.FilterProperty, property.Name,
                    StringComparison.OrdinalIgnoreCase))
                filter = f => GetPropValue(f, property.Name).Equals(request.FilterValue);

            // Find if any property name is matching with OrderBy input
            if (request.OrderBy is not null &&
                string.Equals(request.OrderBy, property.Name, StringComparison.OrdinalIgnoreCase))
                orderBy = o => GetPropValue(o, property.Name).ToString();
        }

        return await _employeeRepository.GetAsync(cancellationToken, filter, orderBy, request.OrderByDesc);
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}