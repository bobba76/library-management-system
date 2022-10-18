using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Library.Domain.LibraryItemAggregate;
using MediatR;

namespace Library.Infrastructure.Repositories;

public class LibraryItemRepository : SQLRepository<LibraryItem>, ILibraryItemRepository
{
    private const int IdIndex = 0;
    private const int CategoryIdIndex = 1;
    private const int TitleIndex = 2;
    private const int AuthorIndex = 3;
    private const int PagesIndex = 4;
    private const int RunTimeMinutesIndex = 5;
    private const int IsBorrowableIndex = 6;
    private const int BorrowerIndex = 7;
    private const int BorrowDateIndex = 8;
    private const int TypeIndex = 9;

    public LibraryItemRepository(IMediator mediator) : base(mediator)
    {
    }

    public override async Task<IEnumerable<LibraryItem>> GetAsync(CancellationToken cancellationToken,
        Expression<Func<LibraryItem, bool>>? filter = null, Expression<Func<LibraryItem, string>>? orderBy = null,
        bool? orderByDesc = false)
    {
        var libraryItems = new List<LibraryItem>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string commandText = "SELECT * FROM library_items";
        await using var cmd = new SqlCommand(commandText, connection);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            LibraryItem libraryItem = new()
            {
                Id = reader.GetInt32(IdIndex),
                CategoryId = reader.GetInt32(CategoryIdIndex),
                Title = reader.GetString(TitleIndex),
                Author = reader.GetString(AuthorIndex),
                Pages = reader.GetInt32(PagesIndex),
                RunTimeMinutes = reader.GetInt32(RunTimeMinutesIndex),
                IsBorrowable = reader.GetBoolean(IsBorrowableIndex),
                Borrower = reader.GetString(BorrowerIndex),
                BorrowDate = reader.IsDBNull(BorrowDateIndex)
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(BorrowDateIndex)),
                Type = reader.GetFieldValue<LibraryItemType>(TypeIndex)
            };

            libraryItems.Add(libraryItem);
        }

        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetAsync)}')");


        if (filter is not null && orderBy is not null && orderByDesc is not null)
            return libraryItems.AsQueryable().Where(filter).OrderByDescending(orderBy);

        if (filter is not null && orderBy is not null) return libraryItems.AsQueryable().Where(filter).OrderBy(orderBy);

        if (filter is not null) return libraryItems.AsQueryable().Where(filter);

        if (orderBy is not null && orderByDesc is not false)
            return libraryItems.AsQueryable().OrderByDescending(orderBy);

        if (orderBy is not null) return libraryItems.AsQueryable().OrderBy(orderBy);

        return libraryItems;
    }

    public override async Task<LibraryItem> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var libraryItem = new LibraryItem();

        const string commandText = "SELECT * FROM library_items WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            libraryItem.Id = reader.GetInt32(IdIndex);
            libraryItem.CategoryId = reader.GetInt32(CategoryIdIndex);
            libraryItem.Title = reader.GetString(TitleIndex);
            libraryItem.Author = reader.GetString(AuthorIndex);
            libraryItem.Pages = reader.GetInt32(PagesIndex);
            libraryItem.RunTimeMinutes = reader.GetInt32(RunTimeMinutesIndex);
            libraryItem.IsBorrowable = reader.GetBoolean(IsBorrowableIndex);
            libraryItem.Borrower = reader.GetString(BorrowerIndex);
            libraryItem.BorrowDate = reader.IsDBNull(BorrowDateIndex)
                ? null
                : DateOnly.FromDateTime(reader.GetDateTime(BorrowDateIndex));
            libraryItem.Type = reader.GetFieldValue<LibraryItemType>(TypeIndex);
        }

        if (!reader.HasRows)
            throw new ArgumentException(
                $"There was no match in database. (Command: '{nameof(GetByIdAsync)}', Parameter '{nameof(id)}', Value '{id}')");

        return libraryItem;
    }

    public override async Task CreateAsync(LibraryItem entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
        INSERT INTO library_items 
        VALUES(@categoryId @title, @author, @pages, @runTimeMinutes, @isBorrowable, @borrower, @borrowDate, @type)
        ";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.NVarChar)).Value = entity.CategoryId;
        cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar)).Value = entity.Title;
        cmd.Parameters.Add(new SqlParameter("@author", SqlDbType.NVarChar)).Value = entity.Author;
        cmd.Parameters.Add(new SqlParameter("@pages", SqlDbType.Int)).Value =
            entity.Pages.GetValueOrDefault(0) == 0 ? DBNull.Value : entity.Pages;
        cmd.Parameters.Add(new SqlParameter("@runTimeMinutes", SqlDbType.Int)).Value =
            entity.RunTimeMinutes.GetValueOrDefault(0) == 0 ? DBNull.Value : entity.RunTimeMinutes;
        cmd.Parameters.Add(new SqlParameter("@isBorrowable", SqlDbType.Bit)).Value = entity.IsBorrowable;
        cmd.Parameters.Add(new SqlParameter("@borrower", SqlDbType.NVarChar)).Value = entity.Borrower;
        cmd.Parameters.Add(new SqlParameter("@borrowDate", SqlDbType.Date)).Value = entity.BorrowDate;
        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)).Value = (int) entity.Type;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task UpdateAsync(LibraryItem entity, CancellationToken cancellationToken)
    {
        const string commandText = @"
            UPDATE library_items
            SET
            category_id = COALESCE(@categoryId, category_id),
            title = COALESCE(@title, title),
            author = COALESCE(@author, author),
            pages = COALESCE(@pages, pages),
            run_time_minutes = COALESCE(@runTimeMinutes, run_time_minutes),
            is_borrowable = COALESCE(@isBorrowable, is_borrowable),
            borrower = @borrower,
            borrow_date = @borrowDate,
            type = @type
            WHERE id = @id";


        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = entity.Id;
        cmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.NVarChar)).Value = entity.CategoryId;
        cmd.Parameters.Add(new SqlParameter("@title", SqlDbType.NVarChar)).Value = entity.Title;
        cmd.Parameters.Add(new SqlParameter("@author", SqlDbType.NVarChar)).Value = entity.Author;
        cmd.Parameters.Add(new SqlParameter("@pages", SqlDbType.Int)).Value =
            entity.Pages.GetValueOrDefault(0) == 0 ? DBNull.Value : entity.Pages;
        cmd.Parameters.Add(new SqlParameter("@runTimeMinutes", SqlDbType.Int)).Value =
            entity.RunTimeMinutes.GetValueOrDefault(0) == 0 ? DBNull.Value : entity.RunTimeMinutes;
        cmd.Parameters.Add(new SqlParameter("@isBorrowable", SqlDbType.Bit)).Value = entity.IsBorrowable;
        cmd.Parameters.Add(new SqlParameter("@borrower", SqlDbType.NVarChar)).Value = entity.Borrower;
        cmd.Parameters.Add(new SqlParameter("@borrowDate", SqlDbType.Date)).Value = entity.BorrowDate;
        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)).Value = (int) entity.Type;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public override async Task DeleteAsync(LibraryItem entity, CancellationToken cancellationToken)
    {
        const string commandText = "DELETE FROM library_items WHERE id = @id";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var cmd = new SqlCommand(commandText, connection);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = entity.Id;

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}