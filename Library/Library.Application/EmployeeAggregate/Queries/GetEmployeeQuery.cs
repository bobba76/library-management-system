using Library.Domain.EmployeeAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.EmployeeAggregate.Queries;

public class GetEmployeeQuery : IQuery<EmployeeBase>
{
    public Guid Id { get; set; }
}