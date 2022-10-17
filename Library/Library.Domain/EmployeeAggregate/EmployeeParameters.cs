namespace Library.Domain.EmployeeAggregate;

public class EmployeeCreateParameters
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int SalaryInput { get; set; }
    public EmployeeRole Role { get; set; }
    public string? ManagerId { get; set; }
}

public class EmployeeUpdateParameters
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int SalaryInput { get; set; }
    public EmployeeRole Role { get; set; }
    public string? ManagerId { get; set; }
}