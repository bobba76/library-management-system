using AutoMapper;
using Library.Domain.EmployeeAggregate;
using Library.Domain.EmployeeAggregate.Roles;
using MediatR;

namespace Library.Application.EmployeeAggregate.Commands;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, IEnumerable<EmployeeBase>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeBase>> Handle(CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAsync(cancellationToken);

        switch (request.Role)
        {
            case EmployeeRole.CEO:
            {
                var ceoExists = employees.Any(e => e.Role.Equals(EmployeeRole.CEO));

                if (ceoExists)
                    throw new ArgumentException(
                        $"Employee cannot have role {EmployeeRole.CEO}. A {EmployeeRole.CEO} already exists. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }

            case EmployeeRole.Employee:
            default:
            {
                var mangerIsCeo = employees.Any(e =>
                    e.Role.Equals(EmployeeRole.CEO) &&
                    !string.IsNullOrWhiteSpace(request.ManagerId) &&
                    e.Id.ToString().Equals(request.ManagerId));

                if (mangerIsCeo)
                    throw new ArgumentException(
                        $"Employee cannot have {EmployeeRole.CEO} as Manager. Only employees with role {EmployeeRole.Manager} can. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }
        }

        var managerIsEmployee = employees.Any(e =>
            e.Role.Equals(EmployeeRole.Employee) &&
            !string.IsNullOrWhiteSpace(request.ManagerId) &&
            e.Id.ToString().Equals(request.ManagerId));

        if (managerIsEmployee)
            throw new ArgumentException(
                $"Employee cannot have {EmployeeRole.Employee} as Manager. An employee with role {EmployeeRole.Employee} cannot manage other employees. (Parameter '{nameof(request.ManagerId)}, Value '{request.ManagerId}')");

        var employee = CreateEmployeeBasedOfRole(request);

        await _employeeRepository.CreateAsync(employee, cancellationToken);

        return await _employeeRepository.GetAsync(cancellationToken);
    }

    private EmployeeBase CreateEmployeeBasedOfRole(CreateEmployeeCommand request)
    {
        var employee = _mapper.Map<EmployeeCreateParameters>(request);

        switch (employee.Role)
        {
            case EmployeeRole.CEO:
                return CEO.Create(employee);

            case EmployeeRole.Manager:
                return Manager.Create(employee);

            case EmployeeRole.Employee:
            default:
                return Employee.Create(employee);
        }
    }
}