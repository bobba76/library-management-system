namespace Library.Domain.EmployeeAggregate.Roles;

public class Ceo : EmployeeBase
{
    public static float SalaryCoefficient => 2.725F;

    public new static Ceo Create(CreateEmployeeParameters parameters)
    {
        EmployeeBase.Create(parameters);

        return Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.Salary,
            parameters.ManagerId
        );
    }

    private static Ceo Create(string firstName, string lastName,
        int salary, int? managerId)
    {
        if (managerId.GetValueOrDefault(0) != 0)
            throw new ArgumentException(
                $"Value must be null. {EmployeeRole.Ceo} cannot have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}, Value '{managerId}'')");

        Ceo employee = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salary * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Ceo
        };

        return employee;
    }

    public new static Ceo Update(UpdateEmployeeParameters parameters)
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

    private static Ceo Update(int id, string? firstName, string? lastName, int salary, int? managerId)
    {
        if (managerId.GetValueOrDefault(0) != 0)
            throw new ArgumentException(
                $"Value must be null. {EmployeeRole.Ceo} cannot have a {EmployeeRole.Manager}. (Parameter '{nameof(managerId)}', Value '{managerId}')");

        Ceo employee = new()
        {
            Id = id,
            Salary = salary * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Ceo
        };

        if (firstName is not null)
            employee.FirstName = firstName.Trim();

        if (lastName is not null)
            employee.LastName = lastName.Trim();

        return employee;
    }
}