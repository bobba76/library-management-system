namespace Library.Domain.LibraryItemAggregate.Types;

public class ReferenceBook : LibraryItem
{
    public new static ReferenceBook Create(CreateLibraryItemParameters parameters)
    {
        LibraryItem.Create(parameters);

        return Create(
            parameters.CategoryId,
            parameters.Title,
            parameters.Author,
            parameters.Pages
        );
    }

    private static ReferenceBook Create(int categoryId, string title, string? author, int? pages)
    {
        // TODO: Checka vad som händer vid null
        if (author?.Trim().Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters. (Parameter '{nameof(author)}', Value '{author}')");

        if (pages < 1)
            throw new ArgumentException(
                $"Value must be more than 0. (Parameter '{nameof(pages)}', Value '{pages}')");

        ReferenceBook libraryItem = new()
        {
            CategoryId = categoryId,
            Title = title,
            Pages = pages,
            IsBorrowable = false,
            Type = LibraryItemType.ReferenceBook
        };

        return libraryItem;
    }

    public new static ReferenceBook Update(UpdateLibraryItemParameters parameters)
    {
        LibraryItem.Update(parameters);

        return Update(
            parameters.Id,
            parameters.CategoryId,
            parameters.Title,
            parameters.Author,
            parameters.Pages,
            parameters.BorrowDate,
            parameters.Borrower
        );
    }

    private static ReferenceBook Update(int id, int? categoryId, string? title, string? author, int? pages,
        DateTime? borrowDate, string? borrower)
    {
        ReferenceBook libraryItem = new()
        {
            Id = id,
            IsBorrowable = false,
            Type = LibraryItemType.ReferenceBook
        };

        if (categoryId is not null)
            libraryItem.CategoryId = categoryId;

        if (title is not null)
            libraryItem.Title = title.Trim();

        if (author is not null)
            libraryItem.Author = author.Trim();

        if (pages is not null)
            libraryItem.Pages = pages;

        if (borrowDate is not null)
            libraryItem.BorrowDate = borrowDate;

        if (borrower is not null)
            libraryItem.Borrower = borrower;

        return libraryItem;
    }
}