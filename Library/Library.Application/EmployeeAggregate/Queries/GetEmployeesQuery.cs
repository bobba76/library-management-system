using System.ComponentModel;
using Library.Domain.EmployeeAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.EmployeeAggregate.Queries;

public class GetEmployeesQuery : IQuery<IEnumerable<EmployeeBase>>
{
    // The queries are case-sensitive
    // FirstName
    public string? OrderBy { get; set; } = null;

    public bool? OrderByDesc { get; set; } = false;

    // FirstName, lastname ...
    public string? FilterProperty { get; set; }

    // Filip, Svensson ...
    public string? FilterValue { get; set; }
}
