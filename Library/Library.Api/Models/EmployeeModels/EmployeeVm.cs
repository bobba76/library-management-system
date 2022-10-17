using AutoMapper;
using Library.Application.Common;
using Library.Domain.EmployeeAggregate;

namespace Library.Api.Models.EmployeeModels;

public class EmployeeVm : IMapFrom<EmployeeBase>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Salary { get; set; }
    public EmployeeRole Role { get; set; }
    public string ManagerId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EmployeeBase, EmployeeVm>();
    }
}