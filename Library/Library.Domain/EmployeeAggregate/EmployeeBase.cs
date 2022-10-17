using Library.SharedKernel;

namespace Library.Domain.EmployeeAggregate;

public class EmployeeBase : Entity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public decimal Salary { get; set; }

    public EmployeeRole Role { get; set; }

    public string? ManagerId { get; set; }

    protected static void Create(EmployeeCreateParameters parameters)
    {
        Create(
            parameters.FirstName,
            parameters.LastName,
            parameters.SalaryInput,
            parameters.Role,
            parameters.ManagerId
        );
    }

    private static void Create(string firstName, string lastName,
        int salaryInput, EmployeeRole role, string? managerId)
    {
        if (firstName.Trim().Length is < 1 or > 255)
            throw new ArgumentException(
                $"Value must be between 1 - 255 characters. (Parameter '{nameof(firstName)}', Value '{firstName}')");

        if (lastName.Trim().Length is < 1 or > 255)
            throw new ArgumentException(
                $"Value must be between 1 - 255 characters. (Parameter '{nameof(lastName)}', Value '{lastName}')");

        if (salaryInput is < 1 or > 10)
            throw new ArgumentException(
                $"Value must be an int between 1 - 10. (Parameter '{nameof(salaryInput)}', Value '{salaryInput}')");

        if (!Enum.IsDefined(typeof(EmployeeRole), role))
            throw new ArgumentException(
                $"Value must be typeof EmployeeRole. (Parameter '{nameof(role)}', Value '{role}')");
        
        if (!Guid.TryParse(managerId, out var managerGuid) && !string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException(
                $"Value must be typeof Guid OR null. (Parameter '{nameof(managerId)}', Value '{managerId}')");

        if (managerGuid.Equals(Guid.Empty) && !string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException(
                $"Value cannot be equal to Guid Empty. (Parameter '{nameof(managerId)}', Value '{managerId}')");
    }

    protected static void Update(EmployeeUpdateParameters parameters)
    {
        Update(
            parameters.FirstName,
            parameters.LastName,
            parameters.SalaryInput,
            parameters.Role,
            parameters.ManagerId
        );
    }

    // TODO: Checka vad som händer om man skickar in Employee med GUID 000-00000000000000-00000
    private static void Update(string? firstName, string? lastName, int salaryInput, EmployeeRole role,
        string? managerId)
    {
        if (firstName?.Trim().Length is < 1 or > 255)
            throw new ArgumentException(
                $"Value must be null or between 1 - 255 characters. (Parameter '{nameof(firstName)}', Value '{firstName}')");

        if (lastName?.Trim().Length is < 1 or > 255)
            throw new ArgumentException(
                $"Value must be null or between 1 - 255 characters. (Parameter '{nameof(lastName)}', Value '{lastName}')");

        if (salaryInput is < 1 or > 10)
            throw new ArgumentException(
                $"Value must be an int between 1 - 10. (Parameter '{nameof(salaryInput)}', Value '{salaryInput}')");

        if (!Enum.IsDefined(typeof(EmployeeRole), role))
            throw new ArgumentException(
                $"Value must be of EmployeeRole. (Parameter '{nameof(role)}', Value '{role}')");

        if (!Guid.TryParse(managerId, out var managerGuid) && managerGuid.Equals(Guid.Empty)  && !string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException(
                $"Value must be null or a non-nullable Guid. (Parameter '{nameof(managerId)}', Value '{managerId} {managerGuid}')");

        if (managerGuid.Equals(Guid.Empty) && !string.IsNullOrWhiteSpace(managerId))
            throw new ArgumentException(
                $"Value cannot be equal to Guid Empty. (Parameter '{nameof(managerId)}', Value '{managerId}')");
    }
}