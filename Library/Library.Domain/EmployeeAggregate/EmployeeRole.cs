using System.ComponentModel;

namespace Library.Domain.EmployeeAggregate;

public enum EmployeeRole
{
    [Description("Employee")] Employee = 1,

    [Description("Manager")] Manager = 2,

    [Description("CEO")] CEO = 3
}