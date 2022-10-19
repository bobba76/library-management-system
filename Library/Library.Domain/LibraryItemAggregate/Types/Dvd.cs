namespace Library.Domain.LibraryItemAggregate.Types;

public class Dvd : LibraryItem
{
    public new static Dvd Create(CreateLibraryItemParameters parameters)
    {
        LibraryItem.Create(parameters);

        return Create(
            parameters.CategoryId,
            parameters.Title,
            parameters.RunTimeMinutes
        );
    }

    private static Dvd Create(int categoryId, string title, int? runTimeMinutes)
    {
        if (runTimeMinutes < 1)
            throw new ArgumentException(
                $"Value must be more than 0. (Parameter '{nameof(runTimeMinutes)}', Value '{runTimeMinutes}')");

        Dvd libraryItem = new()
        {
            CategoryId = categoryId,
            Title = title,
            RunTimeMinutes = runTimeMinutes,
            IsBorrowable = true,
            Type = LibraryItemType.Dvd
        };

        return libraryItem;
    }

    public new static Dvd Update(UpdateLibraryItemParameters parameters)
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

    private static Dvd Update(int id, int? categoryId, string? title, int? runTimeMinutes, DateTime? borrowDate,
        string? borrower)
    {
        Dvd libraryItem = new()
        {
            Id = id,
            IsBorrowable = true,
            Type = LibraryItemType.Dvd
        };

        if (categoryId is not null)
            libraryItem.CategoryId = categoryId;

        if (title is not null)
            libraryItem.Title = title.Trim();

        // TODO: Checka om is not null endast är värden över 0. Eller om alla värden, inklusive 0 och negativa räknas som true
        if (runTimeMinutes is not null)
            libraryItem.RunTimeMinutes = runTimeMinutes;

        if (borrowDate is not null)
            libraryItem.BorrowDate = borrowDate;

        if (borrower is not null)
            libraryItem.Borrower = borrower;

        return libraryItem;
    }
}