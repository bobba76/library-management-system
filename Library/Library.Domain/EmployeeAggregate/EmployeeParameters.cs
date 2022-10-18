namespace Library.Domain.EmployeeAggregate;

public class CreateEmployeeParameters
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Salary { get; set; }
    public EmployeeRole Role { get; set; }
    public int? ManagerId { get; set; }
}

public class UpdateEmployeeParameters
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Salary { get; set; }
    public EmployeeRole Role { get; set; }
    public int? ManagerId { get; set; }
}