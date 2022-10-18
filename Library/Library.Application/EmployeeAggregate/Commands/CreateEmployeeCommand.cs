﻿using Library.Domain.EmployeeAggregate;
using Library.SharedKernel.Interfaces;

namespace Library.Application.EmployeeAggregate.Commands;

public class CreateEmployeeCommand : ICommand<IEnumerable<EmployeeBase>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int SalaryInput { get; set; }
    public EmployeeRole Role { get; set; }
    public string? ManagerId { get; set; }
}