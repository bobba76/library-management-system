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

            cmd.CommandText = "DROP TABLE IF EXISTS employees";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE employees(
                id UNIQUEIDENTIFIER PRIMARY KEY,
	            first_name VARCHAR (255) NOT NULL,
	            last_name VARCHAR (255) NOT NULL,
	            salary DECIMAL (5,3) NOT NULL,
	            role INT NOT NULL,
	            manager_id VARCHAR (36),
            )";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT INTO employees(id, first_name, last_name, salary, role, manager_id) VALUES
                ('89c631fc-6a77-44b9-936f-ccbd196d78a4', 'Bill', 'Gates', 27.25, 3, NULL),
                ('f130446e-ce46-4a5b-a49f-931cfb63e8e9', 'Elon', 'Musk', 15.525, 2, '89c631fc-6a77-44b9-936f-ccbd196d78a4'),
                ('cb89834b-2fc5-46a1-bee5-bf0c30815a11', 'Will', 'Smith', 5.175, 2, 'f130446e-ce46-4a5b-a49f-931cfb63e8e9'),
                ('c41ec387-a882-423e-aff5-8dc5509387fd', 'Robin', 'Lindgren', 10.35, 2, NULL),
                (NEWID(), 'Bob', 'Marley', 1.125, 1, 'c41ec387-a882-423e-aff5-8dc5509387fd'),
                (NEWID(), 'Cristiano', 'Ronaldo', 11.25, 1, 'c41ec387-a882-423e-aff5-8dc5509387fd'),
                (NEWID(), 'Boris', 'Johnson', 4.5, 1, 'f130446e-ce46-4a5b-a49f-931cfb63e8e9')";
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