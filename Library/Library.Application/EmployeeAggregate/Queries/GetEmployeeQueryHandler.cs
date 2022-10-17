using Library.Domain.EmployeeAggregate;
using MediatR;

namespace Library.Application.EmployeeAggregate.Queries;

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeBase>
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeBase> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        return await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}