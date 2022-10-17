namespace Library.Domain.EmployeeAggregate.Roles;

public class CEO : EmployeeBase
{
    public new static float SalaryCoefficient => 2.725F;

    public new static CEO Create(EmployeeCreateParameters parameters)
    {
        EmployeeBase.Create(parameters);

        return Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.SalaryInput,
            parameters.ManagerId
        );
    }

    private static CEO Create(string firstName, string lastName,
        int salaryInput, string? managerId)
    {
        if (managerId is not null)
            throw new ArgumentException(
                $"Value must be null. {EmployeeRole.CEO} cannot have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}, Value '{managerId}'')");

        CEO employee = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salaryInput * (decimal) SalaryCoefficient,
            Role = EmployeeRole.CEO
        };

        return employee;
    }

    public new static CEO Update(EmployeeUpdateParameters parameters)
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

    private static CEO Update(Guid id, string? firstName, string? lastName, int salaryInput, string? managerId)
    {
        if (managerId is not null)
            throw new ArgumentException(
                $"Value must be null. {EmployeeRole.CEO} cannot have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}', Value '{managerId}')");

        CEO employee = new()
        {
            Id = id,
            Salary = salaryInput * (decimal) SalaryCoefficient,
            Role = EmployeeRole.CEO
        };

        if (firstName is not null)
            employee.FirstName = firstName.Trim();

        if (lastName is not null)
            employee.LastName = lastName.Trim();

        return employee;
    }
}