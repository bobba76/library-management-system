using AutoMapper;
using Library.Api.Models.EmployeeModels;
using Library.Application.EmployeeAggregate.Commands;
using Library.Application.EmployeeAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

public class EmployeeController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public EmployeeController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IEnumerable<EmployeeVm>> GetEmployees([FromQuery] GetEmployeesQuery query)
    {
        var employees = await _mediator.Send(query);

        return _mapper.Map<IEnumerable<EmployeeVm>>(employees);
    }

    [HttpGet]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<EmployeeVm> GetEmployeeById(int id)
    {
        var employee = await _mediator.Send(new GetEmployeeQuery {Id = id});

        return _mapper.Map<EmployeeVm>(employee);
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<IEnumerable<EmployeeVm>> CreateEmployee([FromBody] CreateEmployeeInputModel inputModel)
    {
        var command = _mapper.Map<CreateEmployeeCommand>(inputModel);

        var employees = await _mediator.Send(command);

        return _mapper.Map<IEnumerable<EmployeeVm>>(employees);
    }

    [HttpPut]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<EmployeeVm> PutEmployee(int id, [FromBody] UpdateEmployeeInputModel inputModel)
    {
        var command = _mapper.Map<UpdateEmployeeCommand>(inputModel);
        command.Id = id;

        var employee = await _mediator.Send(command);

        return _mapper.Map<EmployeeVm>(employee);
    }

    [HttpDelete]
    [Route("{id}")]
    [Produces("application/json")]
    public async Task<IEnumerable<EmployeeVm>> DeleteEmployee(int id)
    {
        var employees = await _mediator.Send(new DeleteEmployeeCommand {Id = id});

        return _mapper.Map<IEnumerable<EmployeeVm>>(employees);
    }
}