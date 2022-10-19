using Library.Domain.LibraryItemAggregate;

namespace Library.Api.Models.LibraryItemModels;

public class CreateLibraryItemInputModel
{
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Pages { get; set; }
    public int RunTimeMinutes { get; set; }
    public bool IsBorrowable { get; set; }
    public string Borrower { get; set; }
    public string BorrowDate { get; set; }
    public LibraryItemType Type { get; set; }
}