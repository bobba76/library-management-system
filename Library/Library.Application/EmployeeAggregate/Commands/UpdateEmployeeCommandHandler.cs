using AutoMapper;
using Library.Domain.EmployeeAggregate;
using Library.Domain.EmployeeAggregate.Roles;
using MediatR;

namespace Library.Application.EmployeeAggregate.Commands;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeBase>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeBase> Handle(UpdateEmployeeCommand request,
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
                        $"Employee cannot change role to {EmployeeRole.CEO}. A {EmployeeRole.CEO} already exists. (Employee '{request.Id}', Parameter '{nameof(request.Role)}, Value '{request.Role}')");

                var managerToEmployees = employees.Count(e =>
                    e.Role.Equals(EmployeeRole.Employee) &&
                    e.ManagerId != null &&
                    e.ManagerId.Equals(request.Id.ToString()));

                if (managerToEmployees > 0)
                    throw new ArgumentException(
                        $"{EmployeeRole.CEO} cannot manage employees with role {EmployeeRole.Employee}. The person is Manager to {managerToEmployees} employees with the role {EmployeeRole.Employee}.  Remove references before updating employee's role to {EmployeeRole.CEO}. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }

            case EmployeeRole.Employee:
            {
                var mangerIsCeo = employees.Any(e =>
                    e.Role.Equals(EmployeeRole.CEO) &&
                    !string.IsNullOrWhiteSpace(request.ManagerId) &&
                    e.Id.ToString().Equals(request.ManagerId));

                if (mangerIsCeo)
                    throw new ArgumentException(
                        $"Employee cannot have {EmployeeRole.CEO} as Manager. Only employees with role {EmployeeRole.Manager} can. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");

                var managerToAnybody = employees.Count(e =>
                    e.ManagerId != null &&
                    e.ManagerId.Equals(request.Id.ToString()));

                if (managerToAnybody > 0)
                    throw new ArgumentException(
                        $"Employees with role {EmployeeRole.Employee} cannot be Manager to anybody. The person is Manager to {managerToAnybody} employees.  Remove references before updating employee's role to {EmployeeRole.Employee}. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
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

        if (request.ManagerId != null && request.ManagerId.Equals(request.Id.ToString()))
            throw new ArgumentException(
                $"Employee cannot be Manager to themself. (Parameter '{nameof(request.ManagerId)}', Value '{request.ManagerId}', Parameter '{nameof(request.Id)}', Value '{request.Id}'))");


        if (request.Role is null || request.SalaryInput is null)
        {
            var oldEmployee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            // Keeping Role if null
            request.Role ??= oldEmployee.Role;

            // Use the old salary to calculate what the old salaryInput was.
            if (request.SalaryInput is null)
            {
                var salaryCoefficient =
                    oldEmployee.Role switch
                    {
                        EmployeeRole.CEO => CEO.SalaryCoefficient,
                        EmployeeRole.Manager => Manager.SalaryCoefficient,
                        _ => Employee.SalaryCoefficient
                    };

                request.SalaryInput ??= (int) ((float) oldEmployee.Salary / salaryCoefficient);
            }
        }

        var employee = UpdateEmployeeBasedOfRole(request);

        await _employeeRepository.UpdateAsync(employee, cancellationToken);

        return await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
    }

    private EmployeeBase UpdateEmployeeBasedOfRole(UpdateEmployeeCommand request)
    {
        var employee = _mapper.Map<EmployeeUpdateParameters>(request);

        switch (employee.Role)
        {
            case EmployeeRole.CEO:
                return CEO.Update(employee);

            case EmployeeRole.Manager:
                return Manager.Update(employee);

            case EmployeeRole.Employee:
            default:
                return Employee.Update(employee);
        }
    }
}