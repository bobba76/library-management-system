using Library.Domain.LibraryItemAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.LibraryItemAggregate.Queries;

public class GetLibraryItemQuery : IQuery<LibraryItem>
{
    public int Id { get; set; }
}