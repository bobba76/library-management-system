namespace Library.Domain.EmployeeAggregate.Roles;

public class Manager : EmployeeBase
{
    public static float SalaryCoefficient => 1.725F;

    public new static Manager Create(CreateEmployeeParameters parameters)
    {
        EmployeeBase.Create(parameters);

        return Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.Salary,
            parameters.ManagerId
        );
    }

    private static Manager Create(string firstName, string lastName,
        int salary, int? managerId)
    {
        Manager employee = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salary * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Manager
        };

        if (managerId is not null)
            employee.ManagerId = managerId;

        return employee;
    }

    public new static Manager Update(UpdateEmployeeParameters parameters)
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

    private static Manager Update(int id, string? firstName, string? lastName, int salary, int? managerId)
    {
        Manager employee = new()
        {
            Id = id,
            Salary = salary * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Manager
        };

        if (firstName is not null)
            employee.FirstName = firstName;

        if (lastName is not null)
            employee.LastName = lastName;

        if (managerId is not null)
            employee.ManagerId = managerId;

        return employee;
    }
}