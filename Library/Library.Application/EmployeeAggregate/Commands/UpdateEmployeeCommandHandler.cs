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
            case EmployeeRole.Ceo:
            {
                var ceoExists = employees.Any(e => e.Role.Equals(EmployeeRole.Ceo));

                if (ceoExists)
                    throw new ArgumentException(
                        $"Employee cannot change role to {EmployeeRole.Ceo}. A {EmployeeRole.Ceo} already exists. (Employee '{request.Id}', Parameter '{nameof(request.Role)}, Value '{request.Role}')");

                var managerToEmployees = employees.Count(e =>
                    e.Role.Equals(EmployeeRole.Employee) &&
                    e.ManagerId.GetValueOrDefault(0) != 0 &&
                    e.ManagerId.Equals(request.Id));

                if (managerToEmployees > 0)
                    throw new ArgumentException(
                        $"{EmployeeRole.Ceo} cannot manage employees with role {EmployeeRole.Employee}. The person is Manager to {managerToEmployees} employees with the role {EmployeeRole.Employee}.  Remove references before updating employee's role to {EmployeeRole.Ceo}. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }

            case EmployeeRole.Employee:
            {
                var mangerIsCeo = employees.Any(e =>
                    e.Role.Equals(EmployeeRole.Ceo) &&
                    request.ManagerId.GetValueOrDefault(0) != 0 &&
                    e.Id.Equals(request.ManagerId));

                if (mangerIsCeo)
                    throw new ArgumentException(
                        $"Employee cannot have {EmployeeRole.Ceo} as Manager. Only employees with role {EmployeeRole.Manager} can. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");

                var managerToAnybody = employees.Count(e =>
                    e.ManagerId.GetValueOrDefault(0) != 0 &&
                    e.ManagerId.Equals(request.Id));

                if (managerToAnybody > 0)
                    throw new ArgumentException(
                        $"Employees with role {EmployeeRole.Employee} cannot be Manager to anybody. The person is Manager to {managerToAnybody} employees.  Remove references before updating employee's role to {EmployeeRole.Employee}. (Parameter '{nameof(request.Role)}, Value '{request.Role}')");
                break;
            }
        }

        var managerIsEmployee = employees.Any(e =>
            e.Role.Equals(EmployeeRole.Employee) &&
            request.ManagerId.GetValueOrDefault(0) != 0 &&
            e.Id.Equals(request.ManagerId));

        if (managerIsEmployee)
            throw new ArgumentException(
                $"Employee cannot have {EmployeeRole.Employee} as Manager. An employee with role {EmployeeRole.Employee} cannot manage other employees. (Parameter '{nameof(request.ManagerId)}, Value '{request.ManagerId}')");

        if (request.ManagerId != null && request.ManagerId.Equals(request.Id))
            throw new ArgumentException(
                $"Employee cannot be Manager to themself. (Parameter '{nameof(request.ManagerId)}', Value '{request.ManagerId}', Parameter '{nameof(request.Id)}', Value '{request.Id}'))");


        if (request.Role is null || request.Salary is null)
        {
            var oldEmployee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            // Keeping Role if null
            request.Role ??= oldEmployee.Role;

            // Use the old salary to calculate what the old salary input was.
            if (request.Salary is null)
            {
                var salaryCoefficient =
                    oldEmployee.Role switch
                    {
                        EmployeeRole.Ceo => Ceo.SalaryCoefficient,
                        EmployeeRole.Manager => Manager.SalaryCoefficient,
                        _ => Employee.SalaryCoefficient
                    };

                request.Salary ??= (int) ((float) oldEmployee.Salary / salaryCoefficient);
            }
        }

        var employee = UpdateEmployeeBasedOfRole(_mapper.Map<UpdateEmployeeParameters>(request));

        await _employeeRepository.UpdateAsync(employee, cancellationToken);

        return await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);
    }

    private EmployeeBase UpdateEmployeeBasedOfRole(UpdateEmployeeParameters employee)
    {
        switch (employee.Role)
        {
            case EmployeeRole.Ceo:
                return Ceo.Update(employee);

            case EmployeeRole.Manager:
                return Manager.Update(employee);

            case EmployeeRole.Employee:
            default:
                return Employee.Update(employee);
        }
    }
}