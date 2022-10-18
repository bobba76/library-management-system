using Library.Domain.LibraryItemAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.LibraryItemAggregate.Commands;

public class DeleteLibraryItemCommand : ICommand<IEnumerable<LibraryItem>>
{
    public int Id { get; set; }
}