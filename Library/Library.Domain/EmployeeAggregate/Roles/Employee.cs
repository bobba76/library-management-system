namespace Library.Domain.EmployeeAggregate.Roles;

public class Employee : EmployeeBase
{
    public static float SalaryCoefficient => 1.125F;

    public new static Employee Create(CreateEmployeeParameters parameters)
    {
        EmployeeBase.Create(parameters);

        return Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.Salary,
            parameters.ManagerId
        );
    }

    private static Employee Create(string firstName, string lastName,
        int salary, int? managerId)
    {
        if (managerId.GetValueOrDefault(0) == 0)
            throw new ArgumentException(
                $"Value cannot be null. {EmployeeRole.Employee} must have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}')");

        Employee employee = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salary * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Employee,
            ManagerId = managerId
        };

        return employee;
    }

    public new static Employee Update(UpdateEmployeeParameters parameters)
    {
        EmployeeBase.Update(parameters);

        return Update(
            parameters.Id,
            parameters.FirstName,
            parameters.LastName,
            parameters.Salary,
            parameters.ManagerId
        );
    }

    private static Employee Update(int id, string? firstName, string? lastName, int salary, int? managerId)
    {
        if (managerId.GetValueOrDefault(0) == 0)
            throw new ArgumentException(
                $"Value cannot be null. {EmployeeRole.Employee} must have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}')");

        Employee employee = new()
        {
            Id = id,
            Salary = salary * (decimal) SalaryCoefficient,
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