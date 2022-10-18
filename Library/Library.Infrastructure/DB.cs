using System.Data.SqlClient;

namespace Library.Infrastructure;

public class DB
{
    // Update with your SQL Server connection string.
    internal static string ConnectionString =
        "Data Source=DESKTOP-E27PMGN\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    public static string UpdateConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
        return "ConnectionString updated.";
    }

    public static string Setup()
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = @"
            DROP TABLE IF EXISTS library_item;
            DROP TABLE IF EXISTS category;
            DROP TABLE IF EXISTS employees;
            ";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"
            CREATE TABLE library_item(
                id INT IDENTITY(1,1) PRIMARY KEY,
	            category_id INT NOT NULL FOREIGN KEY REFERENCES category(id),
	            title NVARCHAR (64) NOT NULL,
                author NVARCHAR (64),
                pages INT,
                run_times_minutes INT,
                is_borrowable BIT NOT NULL,
                borrower NVARCHAR (64),
                borrow_date DATE,
                type INT NOT NULL
            );

            CREATE TABLE category(
                id INT IDENTITY(1,1) PRIMARY KEY,
	            category_name NVARCHAR (64) NOT NULL UNIQUE
            );

            CREATE TABLE employees(
                id INT IDENTITY(1,1) PRIMARY KEY,
	            first_name NVARCHAR (64) NOT NULL,
	            last_name NVARCHAR (64) NOT NULL,
	            salary DECIMAL (5,3) NOT NULL,
	            role INT NOT NULL,
	            manager_id INT
            );
            ";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"
            INSERT INTO employees(category_id, title, author, pages, run_times_minutes, is_borrowable, borrower, borrow_date, type) VALUES
            (1, 'Harry Potter', 'J. K. Rowling', 223, null, 1, null, null, 1),
            (1, 'The Witcher', 'Andrzej Sapkowski', 288, null, 1, null, null, 1),
            (2, 'Picture You Dead', 'Peter James', 400, null, 1, 'Robin Lindgren', '2022-10-14', 1),
            (3, 'The Notebook', null, null, 124, 1, null, null, 2),
            (6, 'IT', null, null, null, 135, 1, 'Robin Lindgren', '2022-09-20', 3),
            (4, 'Gone Girl', null, null, 1197, 1, null, null, 3),
            (8, 'Svensk ordbok', 'Svenska Akademien', 1473, null, 0, null, null, 4)

            INSERT INTO category(category_name) VALUES
            ('Fantasy'),
            ('Crime'),
            ('Romance'),
            ('Thrillers'),
            ('War'),
            ('Horror'),
            ('Biography'),
            ('Self Improvement'),
            ('Adventure'),
            ('Mystery');

            INSERT INTO employees(first_name, last_name, salary, role, manager_id) VALUES
            ('Bill', 'Gates', 27.25, 3, 0),
            ('Elon', 'Musk', 15.525, 2, 1),
            ('Will', 'Smith', 5.175, 2, 2),
            ('Robin', 'Lindgren', 10.35, 2, 0),
            ('Bob', 'Marley', 1.125, 1, 4),
            ('Cristiano', 'Ronaldo', 11.25, 1, 4),
            ('Boris', 'Johnson', 4.5, 1, 0);
            ";
            cmd.ExecuteNonQuery();

            return "Data created.";
        }

        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static string DropTable()
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = "DROP TABLE employees";
            cmd.ExecuteNonQuery();

            return "Table removed.";
        }

        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public static string RemoveData()
    {
        try
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            using var cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = "DELETE FROM employees";
            cmd.ExecuteNonQuery();

            return "Data removed.";
        }

        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}