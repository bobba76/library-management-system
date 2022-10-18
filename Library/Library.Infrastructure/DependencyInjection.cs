using Library.Domain.CategoryAggregate;
using Library.Domain.EmployeeAggregate;
using Library.Domain.LibraryItemAggregate;
using Library.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Repositories
        services.AddScoped<ILibraryItemRepository, LibraryItemRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    }
}