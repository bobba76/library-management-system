using Library.SharedKernel;

namespace Library.Domain.EmployeeAggregate;

public class EmployeeBase : Entity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public decimal Salary { get; set; }

    public EmployeeRole Role { get; set; }

    public int? ManagerId { get; set; }

    protected static void Create(CreateEmployeeParameters parameters)
    {
        Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.Salary,
            parameters.Role
        );
    }

    private static void Create(string firstName, string lastName,
        int salary, EmployeeRole role)
    {
        if (firstName.Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters. (Parameter '{nameof(firstName)}', Value '{firstName}')");

        if (lastName.Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters. (Parameter '{nameof(lastName)}', Value '{lastName}')");

        if (salary is < 1 or > 10)
            throw new ArgumentException(
                $"Value must be an int between 1 - 10. (Parameter '{nameof(salary)}', Value '{salary}')");

        if (!Enum.IsDefined(typeof(EmployeeRole), role))
            throw new ArgumentException(
                $"Value must be typeof EmployeeRole. (Parameter '{nameof(role)}', Value '{role}')");
    }

    protected static void Update(UpdateEmployeeParameters parameters)
    {
        Update(
            parameters.FirstName,
            parameters.LastName,
            parameters.Salary,
            parameters.Role
        );
    }

    private static void Update(string? firstName, string? lastName, int salary, EmployeeRole role)
    {
        if (firstName?.Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters OR null. (Parameter '{nameof(firstName)}', Value '{firstName}')");

        if (lastName?.Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters OR null. (Parameter '{nameof(lastName)}', Value '{lastName}')");

        if (salary is < 1 or > 10)
            throw new ArgumentException(
                $"Value must be an int between 1 - 10 OR null. (Parameter '{nameof(salary)}', Value '{salary}')");

        if (!Enum.IsDefined(typeof(EmployeeRole), role))
            throw new ArgumentException(
                $"Value must be of EmployeeRole. (Parameter '{nameof(role)}', Value '{role}')");
    }
}