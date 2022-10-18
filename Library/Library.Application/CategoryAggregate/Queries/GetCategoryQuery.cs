using Library.Domain.CategoryAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.CategoryAggregate.Queries;

public class GetCategoryQuery : IQuery<Category>
{
    public int Id { get; set; }
}