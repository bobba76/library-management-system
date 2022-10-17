using Library.Domain.EmployeeAggregate;

namespace Library.Api.Models.EmployeeModels;

public class UpdateEmployeeInputModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? SalaryInput { get; set; }
    public EmployeeRole? Role { get; set; }
    public string? ManagerId { get; set; }
}