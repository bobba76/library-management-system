using System.ComponentModel;
using Library.Domain.EmployeeAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.EmployeeAggregate.Queries;

public class GetEmployeesQuery : IQuery<IEnumerable<EmployeeBase>>
{
    // FirstName
    public string? OrderBy { get; set; } = null;

    // FirstName, lastname ...
    public string? FilterProperty { get; set; }

    // Filip, Svensson ...
    public string? FilterValue { get; set; }
}
