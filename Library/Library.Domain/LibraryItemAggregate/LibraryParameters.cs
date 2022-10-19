namespace Library.Domain.LibraryItemAggregate;

public class CreateLibraryItemParameters
{
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string? Author { get; set; }
    public int? Pages { get; set; }
    public int? RunTimeMinutes { get; set; }
    public LibraryItemType Type { get; set; }
}

public class UpdateLibraryItemParameters
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? Pages { get; set; }
    public int? RunTimeMinutes { get; set; }
    public string? Borrower { get; set; }
    public DateTime? BorrowDate { get; set; }
    public LibraryItemType Type { get; set; }
}