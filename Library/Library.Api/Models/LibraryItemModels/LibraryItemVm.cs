using AutoMapper;
using Library.Application.Common;
using Library.Domain.LibraryItemAggregate;

namespace Library.Api.Models.LibraryItemModels;

public class LibraryItemVm : IMapFrom<LibraryItem>
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Pages { get; set; }
    public int RunTimeMinutes { get; set; }
    public bool IsBorrowable { get; set; }
    public string Borrower { get; set; }
    public string BorrowDate { get; set; }
    public LibraryItemType Type { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<LibraryItem, LibraryItemVm>();
    }
}