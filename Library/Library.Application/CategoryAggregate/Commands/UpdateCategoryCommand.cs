using Library.Application.Common;
using Library.Domain.CategoryAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.CategoryAggregate.Commands;

public class UpdateCategoryCommand : IdentityUpdateCommand, ICommand<Category>
{
    public string CategoryName { get; set; }
}