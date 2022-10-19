using AutoMapper;
using Library.Api.Models.CategoryModels;
using Library.Application.CategoryAggregate.Commands;
using Library.Application.CategoryAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

public class CategoryController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CategoryController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IEnumerable<CategoryVm>> GetCategories([FromQuery] GetCategoriesQuery query)
    {
        var categories = await _mediator.Send(query);

        return _mapper.Map<IEnumerable<CategoryVm>>(categories);
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<CategoryVm> GetCategory(int id)
    {
        var category = await _mediator.Send(new GetCategoryQuery {Id = id});

        return _mapper.Map<CategoryVm>(category);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<IEnumerable<CategoryVm>> CreateCategory([FromBody] CreateCategoryInputModel inputModel)
    {
        var command = _mapper.Map<CreateCategoryCommand>(inputModel);

        var categories = await _mediator.Send(command);

        return _mapper.Map<IEnumerable<CategoryVm>>(categories);
    }

    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<CategoryVm> PutCategory(int id, [FromBody] UpdateCategoryInputModel inputModel)
    {
        var command = _mapper.Map<UpdateCategoryCommand>(inputModel);
        command.Id = id;

        var category = await _mediator.Send(command);

        return _mapper.Map<CategoryVm>(category);
    }

    [HttpDelete]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IEnumerable<CategoryVm>> DeleteCategory(int id)
    {
        var libraryItems = await _mediator.Send(new DeleteCategoryCommand {Id = id});

        return _mapper.Map<IEnumerable<CategoryVm>>(libraryItems);
    }
}