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
            case EmployeeRole.Ceo:
            {
                var ceoExists = employees.Any(e => e.Role.Equals(EmployeeRole.Ceo));

                if (ceoExists)
                    throw new ArgumentException(
                        $"Employee cannot have role {EmployeeRole.Ceo}. A {EmployeeRole.Ceo} already exists. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }

            case EmployeeRole.Employee:
            {
                var mangerIsCeo = employees.Any(e =>
                    e.Role.Equals(EmployeeRole.Ceo) &&
                    request.ManagerId.GetValueOrDefault(0) == 0 &&
                    e.Id.Equals(request.ManagerId));

                if (mangerIsCeo)
                    throw new ArgumentException(
                        $"Employee cannot have {EmployeeRole.Ceo} as Manager. Only employees with role {EmployeeRole.Manager} can. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }
        }

        var managerIsEmployee = employees.Any(e =>
            e.Role.Equals(EmployeeRole.Employee) &&
            request.ManagerId.GetValueOrDefault(0) == 0 &&
            e.Id.Equals(request.ManagerId));

        if (managerIsEmployee)
            throw new ArgumentException(
                $"Employee cannot have {EmployeeRole.Employee} as Manager. An employee with role {EmployeeRole.Employee} cannot manage other employees. (Parameter '{nameof(request.ManagerId)}, Value '{request.ManagerId}')");

        var employee = CreateEmployeeBasedOfRole(_mapper.Map<CreateEmployeeParameters>(request));

        await _employeeRepository.CreateAsync(employee, cancellationToken);

        return await _employeeRepository.GetAsync(cancellationToken);
    }

    private EmployeeBase CreateEmployeeBasedOfRole(CreateEmployeeParameters employee)
    {
        switch (employee.Role)
        {
            case EmployeeRole.Ceo:
                return Ceo.Create(employee);

            case EmployeeRole.Manager:
                return Manager.Create(employee);

            case EmployeeRole.Employee:
            default:
                return Employee.Create(employee);
        }
    }
}