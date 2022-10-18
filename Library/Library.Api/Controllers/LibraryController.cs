using AutoMapper;
using Library.Api.Models.LibraryItemModels;
using Library.Application.LibraryItemAggregate.Commands;
using Library.Application.LibraryItemAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

public class LibraryItemController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public LibraryItemController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IEnumerable<LibraryItemVm>> GetLibraryItems([FromQuery] GetLibraryItemsQuery query)
    {
        var libraryItems = await _mediator.Send(query);

        return _mapper.Map<IEnumerable<LibraryItemVm>>(libraryItems);
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<LibraryItemVm> GetLibraryItemById(int id)
    {
        var libraryItem = await _mediator.Send(new GetLibraryItemQuery {Id = id});

        return _mapper.Map<LibraryItemVm>(libraryItem);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<IEnumerable<LibraryItemVm>> CreateLibraryItem([FromBody] CreateLibraryItemInputModel inputModel)
    {
        var command = _mapper.Map<CreateLibraryItemCommand>(inputModel);

        var libraryItems = await _mediator.Send(command);

        return _mapper.Map<IEnumerable<LibraryItemVm>>(libraryItems);
    }

    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<LibraryItemVm> PutLibraryItem(int id, [FromBody] UpdateLibraryItemInputModel inputModel)
    {
        var command = _mapper.Map<UpdateLibraryItemCommand>(inputModel);
        command.Id = id;

        var libraryItem = await _mediator.Send(command);

        return _mapper.Map<LibraryItemVm>(libraryItem);
    }

    [HttpDelete]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IEnumerable<LibraryItemVm>> DeleteLibraryItem(int id)
    {
        var libraryItems = await _mediator.Send(new DeleteLibraryItemCommand {Id = id});

        return _mapper.Map<IEnumerable<LibraryItemVm>>(libraryItems);
    }
}