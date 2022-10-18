namespace Library.Domain.CategoryAggregate;

public class CreateCategoryParameters
{
    public string CategoryName { get; set; }
}

public class UpdateCategoryParameters
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
}