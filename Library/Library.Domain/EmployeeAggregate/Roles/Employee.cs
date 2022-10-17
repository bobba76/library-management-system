namespace Library.Domain.EmployeeAggregate.Roles;

public class Employee : EmployeeBase
{
    public new static float SalaryCoefficient => 1.125F;

    public new static Employee Create(EmployeeCreateParameters parameters)
    {
        EmployeeBase.Create(parameters);

        return Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.SalaryInput,
            parameters.ManagerId
        );
    }

    private static Employee Create(string firstName, string lastName,
        int salaryInput, string? managerId)
    {
        if (string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException(
                $"Value cannot be null. {EmployeeRole.Employee} must have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}')");

        Employee employee = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salaryInput * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Employee,
            ManagerId = managerId
        };

        return employee;
    }

    public new static Employee Update(EmployeeUpdateParameters parameters)
    {
        EmployeeBase.Update(parameters);

        return Update(
            parameters.Id,
            parameters.FirstName,
            parameters.LastName,
            parameters.SalaryInput,
            parameters.ManagerId
        );
    }

    private static Employee Update(Guid id, string? firstName, string? lastName, int salaryInput, string? managerId)
    {
        if (string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException(
                $"Value cannot be null. {EmployeeRole.Employee} must have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}')");

        Employee employee = new()
        {
            Id = id,
            Salary = salaryInput * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Employee,
            ManagerId = managerId
        };

        if (firstName is not null)
            employee.FirstName = firstName.Trim();

        if (lastName is not null)
            employee.LastName = lastName.Trim();

        return employee;
    }
}