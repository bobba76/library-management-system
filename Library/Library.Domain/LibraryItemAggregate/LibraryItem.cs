using Library.SharedKernel;

namespace Library.Domain.LibraryItemAggregate;

public class LibraryItem : Entity
{
    public int? CategoryId { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }

    public int? Pages { get; set; }

    public int? RunTimeMinutes { get; set; }

    public bool? IsBorrowable { get; set; }

    public string? Borrower { get; set; }

    public DateTime? BorrowDate { get; set; }

    public LibraryItemType Type { get; set; }

    protected static void Create(CreateLibraryItemParameters parameters)
    {
        Create(
            parameters.Title,
            parameters.Type
        );
    }

    private static void Create(string title, LibraryItemType type)
    {
        if (title.Trim().Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters. (Parameter '{nameof(title)}', Value '{title}')");

        if (!Enum.IsDefined(typeof(LibraryItemType), type))
            throw new ArgumentException(
                $"Value must be typeof {typeof(LibraryItemType)}. (Parameter '{nameof(type)}', Value '{type}')");
    }

    protected static void Update(UpdateLibraryItemParameters parameters)
    {
        Update(
            parameters.Title,
            parameters.Type
        );
    }

    private static void Update(string? title, LibraryItemType type)
    {
        if (!string.IsNullOrWhiteSpace(title) && title.Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters OR null. (Parameter '{nameof(title)}', Value '{title}')");

        if (!Enum.IsDefined(typeof(LibraryItemType), type))
            throw new ArgumentException(
                $"Value must be typeof {typeof(LibraryItemType)}. (Parameter '{nameof(type)}', Value '{type}')");
    }
}