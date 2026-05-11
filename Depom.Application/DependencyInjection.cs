using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Depom.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .Where(type => type.Name.EndsWith("Service")))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}
