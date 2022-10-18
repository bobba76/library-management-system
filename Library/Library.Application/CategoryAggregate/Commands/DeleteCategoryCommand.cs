using Library.Domain.CategoryAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.CategoryAggregate.Commands;

public class DeleteCategoryCommand : ICommand<IEnumerable<Category>>
{
    public int Id { get; set; }
}