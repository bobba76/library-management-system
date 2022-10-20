using System.Reflection;
using AutoMapper;
using Library.Api.Models.CategoryModels;
using Library.Api.Models.EmployeeModels;
using Library.Api.Models.LibraryItemModels;
using Library.Application.CategoryAggregate.Commands;
using Library.Application.Common;
using Library.Application.EmployeeAggregate.Commands;
using Library.Application.LibraryItemAggregate.Commands;
using Library.Domain.CategoryAggregate;
using Library.Domain.EmployeeAggregate;
using Library.Domain.LibraryItemAggregate;

namespace Library.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
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
                // Convert empty strings to nulls and trim the strings
                // Convert int with value 0 and lower to null
                cfg.CreateMap<CreateEmployeeInputModel, CreateEmployeeCommand>()
                    .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s.Trim())
                    .AddTransform<int?>(i => i.GetValueOrDefault(0) <= 0 ? null : i);
                cfg.CreateMap<CreateEmployeeCommand, CreateEmployeeParameters>();
                cfg.CreateMap<UpdateEmployeeInputModel, UpdateEmployeeCommand>()
                    .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s.Trim())
                    .AddTransform<int?>(i => i.GetValueOrDefault(0) <= 0 ? null : i);
                cfg.CreateMap<UpdateEmployeeCommand, UpdateEmployeeParameters>();

                cfg.CreateMap<CreateLibraryItemInputModel, CreateLibraryItemCommand>()
                    .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s.Trim())
                    .AddTransform<int?>(i => i.GetValueOrDefault(0) <= 0 ? null : i);
                cfg.CreateMap<CreateLibraryItemCommand, CreateLibraryItemParameters>();
                cfg.CreateMap<UpdateLibraryItemInputModel, UpdateLibraryItemCommand>()
                    .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s.Trim())
                    .AddTransform<int?>(i => i.GetValueOrDefault(0) <= 0 ? null : i);
                cfg.CreateMap<UpdateLibraryItemCommand, UpdateLibraryItemParameters>();

                cfg.CreateMap<CreateCategoryInputModel, CreateCategoryCommand>()
                    .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s.Trim())
                    .AddTransform<int?>(i => i.GetValueOrDefault(0) <= 0 ? null : i);
                cfg.CreateMap<CreateCategoryCommand, CreateCategoryParameters>();
                cfg.CreateMap<UpdateCategoryInputModel, UpdateCategoryCommand>()
                    .AddTransform<string?>(s => string.IsNullOrWhiteSpace(s) ? null : s.Trim())
                    .AddTransform<int?>(i => i.GetValueOrDefault(0) <= 0 ? null : i);
                cfg.CreateMap<UpdateCategoryCommand, UpdateCategoryParameters>();

                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}