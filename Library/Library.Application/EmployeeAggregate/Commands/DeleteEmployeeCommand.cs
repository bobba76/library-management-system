using Library.Domain.EmployeeAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.EmployeeAggregate.Commands;

public class DeleteEmployeeCommand : ICommand<IEnumerable<EmployeeBase>>
{
    public Guid Id { get; set; }
}