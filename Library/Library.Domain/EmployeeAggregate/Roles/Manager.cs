namespace Library.Domain.EmployeeAggregate.Roles;

public class Manager : EmployeeBase
{
    public new static float SalaryCoefficient => 1.725F;

    public new static Manager Create(EmployeeCreateParameters parameters)
    {
        EmployeeBase.Create(parameters);

        return Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.SalaryInput,
            parameters.ManagerId
        );
    }

    private static Manager Create(string firstName, string lastName,
        int salaryInput, string? managerId)
    {
        Manager employee = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Salary = salaryInput * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Manager
        };

        if (managerId is not null)
            employee.ManagerId = managerId;

        return employee;
    }

    public new static Manager Update(EmployeeUpdateParameters parameters)
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

    private static Manager Update(Guid id, string? firstName, string? lastName, int salaryInput, string? managerId)
    {
        Manager employee = new()
        {
            Id = id,
            Salary = salaryInput * (decimal) SalaryCoefficient,
            Role = EmployeeRole.Manager
        };

        if (firstName is not null)
            employee.FirstName = firstName.Trim();

        if (lastName is not null)
            employee.LastName = lastName.Trim();

        if (managerId is not null)
            employee.ManagerId = managerId;

        return employee;
    }
}