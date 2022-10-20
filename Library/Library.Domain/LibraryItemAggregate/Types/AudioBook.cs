namespace Library.Domain.LibraryItemAggregate.Types;

public class AudioBook : LibraryItem
{
    public new static AudioBook Create(CreateLibraryItemParameters parameters)
    {
        LibraryItem.Create(parameters);

        return Create(
            parameters.CategoryId,
            parameters.Title,
            parameters.RunTimeMinutes
        );
    }

    private static AudioBook Create(int categoryId, string title, int? runTimeMinutes)
    {
        if (runTimeMinutes < 1)
            throw new ArgumentException(
                $"Value must be more than 0. (Parameter '{nameof(runTimeMinutes)}', Value '{runTimeMinutes}')");

        AudioBook libraryItem = new()
        {
            CategoryId = categoryId,
            Title = title,
            RunTimeMinutes = runTimeMinutes,
            IsBorrowable = true,
            Type = LibraryItemType.AudioBook
        };

        return libraryItem;
    }

    public new static AudioBook Update(UpdateLibraryItemParameters parameters)
    {
        LibraryItem.Update(parameters);

        return Update(
            parameters.Id,
            parameters.CategoryId,
            parameters.Title,
            parameters.RunTimeMinutes,
            parameters.BorrowDate,
            parameters.Borrower
        );
    }

    private static AudioBook Update(int id, int? categoryId, string? title, int? runTimeMinutes, DateTime? borrowDate,
        string? borrower)
    {
        AudioBook libraryItem = new()
        {
            Id = id,
            IsBorrowable = true,
            Type = LibraryItemType.AudioBook
        };

        if (categoryId is not null)
            libraryItem.CategoryId = categoryId;

        if (title is not null)
            libraryItem.Title = title;

        if (runTimeMinutes is not null)
            libraryItem.RunTimeMinutes = runTimeMinutes;

        if (borrowDate is not null)
            libraryItem.BorrowDate = borrowDate;

        if (borrower is not null)
            libraryItem.Borrower = borrower;

        return libraryItem;
    }
}