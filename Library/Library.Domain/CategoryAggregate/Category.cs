using Library.SharedKernel;

namespace Library.Domain.CategoryAggregate;

public class Category : Entity
{
    public string CategoryName { get; set; }

    public static Category Create(CreateCategoryParameters parameters)
    {
        return Create(
            parameters.CategoryName
        );
    }

    private static Category Create(string categoryName)
    {
        if (categoryName.Trim().Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters. (Parameter '{nameof(categoryName)}', Value '{categoryName.Trim()}')");

        Category category = new()
        {
            CategoryName = categoryName
        };

        return category;
    }

    public static Category Update(UpdateCategoryParameters parameters)
    {
        return Update(
            parameters.CategoryName
        );
    }

    private static Category Update(string categoryName)
    {
        if (categoryName.Trim().Length is < 1 or > 64)
            throw new ArgumentException(
                $"Value must be between 1 - 64 characters. (Parameter '{nameof(categoryName)}', Value '{categoryName.Trim()}')");

        Category category = new()
        {
            CategoryName = categoryName
        };

        return category;
    }
}