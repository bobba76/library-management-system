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
            if (!string.IsNullOrWhiteSpace(request.FilterProperty) && request.FilterProperty.Equals(property.Name))
                filter = f => GetPropValue(f, property.Name).Equals(request.FilterValue);

            // Find if any property name is matching with OrderBy input
            if (!string.IsNullOrWhiteSpace(request.OrderBy) && request.OrderBy.Equals(property.Name))
                orderBy = o => GetPropValue(o, property.Name).ToString();
        }

        return await _employeeRepository.GetAsync(cancellationToken, filter, orderBy);
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}