using Library.Application.Common;
using Library.Domain.EmployeeAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.EmployeeAggregate.Commands;

public class UpdateEmployeeCommand : IdentityUpdateCommand, ICommand<EmployeeBase>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Salary { get; set; }
    public EmployeeRole? Role { get; set; }
    public int? ManagerId { get; set; }
}