using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Library.Domain.CategoryAggregate;
using MediatR;

namespace Library.Infrastructure.Repositories;

public class CategoryRepository : SQLRepository<Category>, ICategoryRepository
{
    private const int IdIndex = 0;
    private const int CategoryNameIndex = 1;

    public CategoryRepository(IMediator mediator) : base(mediator)
    {
    }

    public override async Task<IEnumerable<Category>> GetAsync(CancellationToken cancellationToken,
        Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, string>>? orderBy = null,
        bool? orderByDesc = false)
    {
        var categories = new List<Category>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = "SELECT * FROM categories";
        await using var cmd = new SqlCommand(commandText, connection);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            Category category = new()
            {
                Id = reader.GetInt32(IdIndex),
                CategoryName = reader.GetString(CategoryNameIndex)
            };

            categories.Add(category);
        }

        /*
        if (!reader.HasRows)
            throw new ArgumentException(
                $"No rows in table 'categories'. (Command: '{nameof(GetAsync)}')");
        */

        if (filter is not null && orderBy is not null && orderByDesc is not null)
            return categories.AsQueryable().Where(filter).OrderByDescending(orderBy);

        if (filter is not null && orderBy is not null) return categories.AsQueryable().Where(filter).OrderBy(orderBy);

        if (filter is not null) return categories.AsQueryable().Where(filter);

        if (orderBy is not null && orderByDesc is not false) return categories.AsQueryable().OrderByDescending(orderBy);

        if (orderBy is not null) return categories.AsQueryable().OrderBy(orderBy);

        return categories;
    }

    public override async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = new Category();

        const string commandText = "SELECT * FROM categories WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            category.Id = reader.GetInt32(IdIndex);
            category.CategoryName = reader.GetString(CategoryNameIndex);
        }

        /* 
        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetByIdAsync)}', Parameter '{nameof(id)}', Value '{id}')");
        */

        return category;
    }

    public override async Task CreateAsync(Category entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
            INSERT INTO categories 
            VALUES(@categoryName)
        ";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.NVarChar)).Value = entity.CategoryName;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task UpdateAsync(Category entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
            UPDATE categories
            SET categoryName = @categoryName,
            WHERE id = @id";


        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = entity.Id;
        cmd.Parameters.Add(new SqlParameter("@categoryName", SqlDbType.VarChar)).Value = entity.CategoryName;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Category entity, CancellationToken cancellationToken)
    {
        const string commandText = "DELETE FROM categories WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = entity.Id;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}