using System.ComponentModel;

namespace Library.Domain.LibraryItemAggregate;

public enum LibraryItemType
{
    [Description("Book")] Book = 1,

    [Description("DVD")] Dvd = 2,

    [Description("Audio Book")] AudioBook = 3,

    [Description("Reference Book")] ReferenceBook = 4
}