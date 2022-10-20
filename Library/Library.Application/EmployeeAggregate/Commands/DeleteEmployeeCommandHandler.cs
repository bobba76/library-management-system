using Library.Domain.EmployeeAggregate;
using MediatR;

namespace Library.Application.EmployeeAggregate.Commands;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, IEnumerable<EmployeeBase>>
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeBase>> Handle(DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

        var employees = await _employeeRepository.GetAsync(cancellationToken);

        var managerToAnybody = employees.Count(e =>
            e.ManagerId is not null &&
            e.ManagerId.Equals(request.Id));

        if (managerToAnybody > 0)
            throw new ArgumentException(
                $"Employee cannot be deleted. The person is Manager to {managerToAnybody} employees. Remove references before deleting employee.");

        await _employeeRepository.DeleteAsync(employee, cancellationToken);

        return await _employeeRepository.GetAsync(cancellationToken);
    }
}