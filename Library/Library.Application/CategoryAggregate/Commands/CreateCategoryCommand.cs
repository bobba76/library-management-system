using Library.Domain.CategoryAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.CategoryAggregate.Commands;

public class CreateCategoryCommand : ICommand<IEnumerable<Category>>
{
    public string CategoryName { get; set; }
}