using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GenocsAspire.Multitenancy.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    }
}