using Library.Domain.LibraryItemAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.LibraryItemAggregate.Commands;

public class CreateLibraryItemCommand : ICommand<IEnumerable<LibraryItem>>
{
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string? Author { get; set; }
    public int? Pages { get; set; }
    public int? RunTimeMinutes { get; set; }
    public LibraryItemType Type { get; set; }
}