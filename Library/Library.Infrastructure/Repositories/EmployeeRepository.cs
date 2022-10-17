using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Library.Domain.EmployeeAggregate;
using MediatR;

namespace Library.Infrastructure.Repositories;

public class EmployeeRepository : SQLRepository<EmployeeBase>, IEmployeeRepository
{
    private const int idIndex = 0;
    private const int firstNameIndex = 1;
    private const int lastNameIndex = 2;
    private const int salary = 3;
    private const int role = 4;
    private const int managerId = 5;

    public EmployeeRepository(IMediator mediator) : base(mediator)
    {
    }

    public override async Task<IEnumerable<EmployeeBase>> GetAsync(CancellationToken cancellationToken,
        Expression<Func<EmployeeBase, bool>>? filter = null, Expression<Func<EmployeeBase, string>>? orderBy = null)
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
                Id = reader.GetGuid(idIndex),
                FirstName = reader.GetString(firstNameIndex),
                LastName = reader.GetString(lastNameIndex),
                Salary = reader.GetDecimal(salary),
                Role = reader.GetFieldValue<EmployeeRole>(role),
                ManagerId = reader.IsDBNull(managerId) ? null : reader.GetString(managerId)
            };

            employees.Add(employee);
        }

        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetAsync)}')");


        if (filter is not null && orderBy is not null) return employees.AsQueryable().Where(filter).OrderBy(orderBy);

        if (filter is not null) return employees.AsQueryable().Where(filter);

        if (orderBy is not null) return employees.AsQueryable().OrderBy(orderBy);

        return employees;
    }

    public override async Task<EmployeeBase> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = new EmployeeBase();

        const string commandText = "SELECT * FROM employees WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier)).Value = id;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Id = reader.GetGuid(idIndex);
            result.FirstName = reader.GetString(firstNameIndex);
            result.LastName = reader.GetString(lastNameIndex);
            result.Salary = reader.GetDecimal(salary);
            result.Role = reader.GetFieldValue<EmployeeRole>(role);
            result.ManagerId = reader.IsDBNull(managerId) ? null : reader.GetString(managerId);
        }

        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetByIdAsync)}', Parameter '{nameof(id)}', Value '{id}')");

        return result;
    }

    public override async Task CreateAsync(EmployeeBase entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
            BEGIN
               IF NOT EXISTS (SELECT * FROM employees 
                               WHERE id = @id)
               BEGIN
                   INSERT INTO employees 
                    VALUES(@id, @firstName, @lastName, @salary, @role, @managerId)
               END
            END
        ";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier)).Value = entity.Id;
        cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.VarChar)).Value = entity.FirstName;
        cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.VarChar)).Value = entity.LastName;
        cmd.Parameters.Add(new SqlParameter("@salary", SqlDbType.Decimal)).Value = entity.Salary;
        cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.Int)).Value = (int) entity.Role;
        cmd.Parameters.Add(new SqlParameter("@managerId", SqlDbType.NVarChar)).Value =
            entity.ManagerId != null ? entity.ManagerId : DBNull.Value;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task UpdateAsync(EmployeeBase entity, CancellationToken cancellationToken)
    {
        var commandText = @"
            UPDATE employees
            SET
            first_name = COALESCE(@firstName, first_name),
            last_name = COALESCE(@lastName, last_name),
            salary = @salary,
            role = @role,
            manager_id = @managerId
            WHERE id = @id";


        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier)).Value = entity.Id;
        cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.VarChar)).Value =
            entity.FirstName == null ? DBNull.Value : entity.FirstName;
        cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.VarChar)).Value =
            entity.LastName == null ? DBNull.Value : entity.LastName;
        cmd.Parameters.Add(new SqlParameter("@salary", SqlDbType.Decimal)).Value = entity.Salary;
        cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.Int)).Value = (int) entity.Role;
        cmd.Parameters.Add(new SqlParameter("@managerId", SqlDbType.VarChar)).Value =
            entity.ManagerId == null ? DBNull.Value : entity.ManagerId;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task DeleteAsync(EmployeeBase entity, CancellationToken cancellationToken)
    {
        const string commandText = "DELETE FROM employees WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier)).Value = entity.Id;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}