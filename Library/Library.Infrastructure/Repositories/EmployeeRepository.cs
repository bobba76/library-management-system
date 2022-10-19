using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Library.Domain.EmployeeAggregate;

namespace Library.Infrastructure.Repositories;

public class EmployeeRepository : SQLRepository<EmployeeBase>, IEmployeeRepository
{
    private const int IdIndex = 0;
    private const int FirstNameIndex = 1;
    private const int LastNameIndex = 2;
    private const int SalaryIndex = 3;
    private const int RoleIndex = 4;
    private const int ManagerIdIndex = 5;

    public override async Task<IEnumerable<EmployeeBase>> GetAsync(CancellationToken cancellationToken,
        Expression<Func<EmployeeBase, bool>>? filter = null, Expression<Func<EmployeeBase, string>>? orderBy = null,
        bool? orderByDesc = false)
    {
        var employees = new List<EmployeeBase>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = "SELECT * FROM employees";
        await using var cmd = new SqlCommand(commandText, connection);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            EmployeeBase employee = new()
            {
                Id = reader.GetInt32(IdIndex),
                FirstName = reader.GetString(FirstNameIndex),
                LastName = reader.GetString(LastNameIndex),
                Salary = reader.GetDecimal(SalaryIndex),
                Role = reader.GetFieldValue<EmployeeRole>(RoleIndex),
                ManagerId = reader.IsDBNull(ManagerIdIndex) ? null : reader.GetInt32(ManagerIdIndex)
            };

            employees.Add(employee);
        }

        /*
        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetAsync)}')");
        */

        if (filter is not null && orderBy is not null && orderByDesc is not null)
            return employees.AsQueryable().Where(filter).OrderByDescending(orderBy);

        if (filter is not null && orderBy is not null) return employees.AsQueryable().Where(filter).OrderBy(orderBy);

        if (filter is not null) return employees.AsQueryable().Where(filter);

        if (orderBy is not null && orderByDesc is not false) return employees.AsQueryable().OrderByDescending(orderBy);

        if (orderBy is not null) return employees.AsQueryable().OrderBy(orderBy);

        return employees;
    }

    public override async Task<EmployeeBase> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var employee = new EmployeeBase();

        const string commandText = "SELECT * FROM employees WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            employee.Id = reader.GetInt32(IdIndex);
            employee.FirstName = reader.GetString(FirstNameIndex);
            employee.LastName = reader.GetString(LastNameIndex);
            employee.Salary = reader.GetDecimal(SalaryIndex);
            employee.Role = reader.GetFieldValue<EmployeeRole>(RoleIndex);
            employee.ManagerId = reader.IsDBNull(ManagerIdIndex) ? null : reader.GetInt32(ManagerIdIndex);
        }

        /*
        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetByIdAsync)}', Parameter '{nameof(id)}', Value '{id}')");
        */

        return employee;
    }

    public override async Task CreateAsync(EmployeeBase entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
            INSERT INTO employees 
            VALUES(@firstName, @lastName, @salary, @role, @managerId)
        ";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar)).Value = entity.FirstName;
        cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar)).Value = entity.LastName;
        cmd.Parameters.Add(new SqlParameter("@salary", SqlDbType.Decimal)).Value = entity.Salary;
        cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.Int)).Value = (int) entity.Role;
        cmd.Parameters.Add(new SqlParameter("@managerId", SqlDbType.Int)).Value =
            entity.ManagerId.GetValueOrDefault(0) == 0 ? DBNull.Value : entity.ManagerId;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task UpdateAsync(EmployeeBase entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
            UPDATE employees
            SET
            first_name = COALESCE(@firstName, first_name),
            last_name = COALESCE(@lastName, last_name),
            salary = @salary,
            role = @role,
            manager_id = @managerId
            WHERE id = @id
        ";


        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = entity.Id;
        cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar)).Value =
            entity.FirstName is null ? DBNull.Value : entity.FirstName;
        cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar)).Value =
            entity.LastName is null ? DBNull.Value : entity.LastName;
        cmd.Parameters.Add(new SqlParameter("@salary", SqlDbType.Decimal)).Value = entity.Salary;
        cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.Int)).Value = (int) entity.Role;
        cmd.Parameters.Add(new SqlParameter("@managerId", SqlDbType.Int)).Value =
            entity.ManagerId.GetValueOrDefault(0) == 0 ? DBNull.Value : entity.ManagerId;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task DeleteAsync(EmployeeBase entity, CancellationToken cancellationToken)
    {
        const string commandText = "DELETE FROM employees WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = entity.Id;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}