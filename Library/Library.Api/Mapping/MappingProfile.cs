using System.Reflection;
using AutoMapper;
using Library.Api.Models.EmployeeModels;
using Library.Application.Common;
using Library.Application.EmployeeAggregate.Commands;
using Library.Application.EmployeeAggregate.Queries;
using Library.Domain.EmployeeAggregate;

namespace Library.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(typeof(IAmApi).Assembly);
    }

    // Only Mapping classes with Interface IMapFrom and Method Mapping.
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                             ?? type.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] {this});
        }
    }

    public static class Mapper
    {
        public static IMapper GetMap()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Employee Mappings
                cfg.CreateMap<CreateEmployeeInputModel, CreateEmployeeCommand>();
                cfg.CreateMap<CreateEmployeeCommand, EmployeeCreateParameters>();
                cfg.CreateMap<UpdateEmployeeInputModel, UpdateEmployeeCommand>();
                cfg.CreateMap<UpdateEmployeeCommand, EmployeeUpdateParameters>();

                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}