using AutoMapper;
using Library.Application.Common;
using Library.Domain.CategoryAggregate;

namespace Library.Api.Models.CategoryModels;

public class CategoryVm : IMapFrom<Category>
{
    public int Id { get; set; }
    public string CategoryName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryVm>();
    }
}