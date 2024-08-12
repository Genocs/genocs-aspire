using GenocsAspire.Multitenancy.Infrastructure;
using GenocsAspire.Multitenancy.Infrastructure.Auth;
using GenocsAspire.Multitenancy.Infrastructure.BackgroundJobs;
using GenocsAspire.Multitenancy.Infrastructure.Caching;
using GenocsAspire.Multitenancy.Infrastructure.Common;
using GenocsAspire.Multitenancy.Infrastructure.Cors;
using GenocsAspire.Multitenancy.Infrastructure.FileStorage;
using GenocsAspire.Multitenancy.Infrastructure.Localization;
using GenocsAspire.Multitenancy.Infrastructure.Mailing;
using GenocsAspire.Multitenancy.Infrastructure.Mapping;
using GenocsAspire.Multitenancy.Infrastructure.Middleware;
using GenocsAspire.Multitenancy.Infrastructure.Multitenancy;
using GenocsAspire.Multitenancy.Infrastructure.Notifications;
using GenocsAspire.Multitenancy.Infrastructure.OpenApi;
using GenocsAspire.Multitenancy.Infrastructure.Persistence;
using GenocsAspire.Multitenancy.Infrastructure.Persistence.Initialization;
using GenocsAspire.Multitenancy.Infrastructure.SecurityHeaders;
using GenocsAspire.Multitenancy.Infrastructure.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace GenocsAspire.Multitenancy.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(Application.Startup).GetTypeInfo().Assembly;
        MapsterSettings.Configure();
        return services
            .AddApiVersioning()
            .AddAuth(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .Behaviors(applicationAssembly)
            .AddHealthCheck()
            .AddPOLocalization(config)
            .AddMailing(config)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
            .AddMultitenancy()
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence()
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

    private static IServiceCollection AddHealthCheck(this IServiceCollection services)
        => services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            .UseMultiTenancy()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/hc");
}