using System.Reflection;
using AutoMapper;
using Library.Application.Common;

using Library.Api.Models.LibraryItemModels;
using Library.Application.LibraryItemAggregate.Commands;
using Library.Domain.LibraryItemAggregate;

using Library.Api.Models.CategoryModels;
using Library.Application.CategoryAggregate.Commands;
using Library.Domain.CategoryAggregate;

using Library.Api.Models.EmployeeModels;
using Library.Application.EmployeeAggregate.Commands;
using Library.Domain.EmployeeAggregate;


namespace Library.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TODO: Testa kommentera ut och se mappning
        ApplyMappingsFromAssembly(typeof(IAmApi).Assembly);
    }

    // Auto-Mapping classes with Interface IMapFrom and Method Mapping.
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

    // Configures auto-mapping between classes.
    public static class Mapper
    {
        public static IMapper GetMap()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateEmployeeInputModel, CreateEmployeeCommand>();
                cfg.CreateMap<CreateEmployeeCommand, CreateEmployeeParameters>();
                cfg.CreateMap<UpdateEmployeeInputModel, UpdateEmployeeCommand>();
                cfg.CreateMap<UpdateEmployeeCommand, UpdateEmployeeParameters>();

                cfg.CreateMap<CreateLibraryItemInputModel, CreateLibraryItemCommand>();
                cfg.CreateMap<CreateLibraryItemCommand, CreateLibraryItemParameters>();
                cfg.CreateMap<UpdateLibraryItemInputModel, UpdateLibraryItemCommand>();
                cfg.CreateMap<UpdateLibraryItemCommand, UpdateLibraryItemParameters>();

                cfg.CreateMap<CreateCategoryInputModel, CreateCategoryCommand>();
                cfg.CreateMap<CreateCategoryCommand, CreateCategoryParameters>();
                cfg.CreateMap<UpdateCategoryInputModel, UpdateCategoryCommand>();
                cfg.CreateMap<UpdateCategoryCommand, UpdateCategoryParameters>();

                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}