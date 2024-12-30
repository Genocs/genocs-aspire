using System.Reflection;
using System.Runtime.CompilerServices;
using Genocs.MultitenancyAspire.Infrastructure.Auth;
using Genocs.MultitenancyAspire.Infrastructure.BackgroundJobs;
using Genocs.MultitenancyAspire.Infrastructure.Caching;
using Genocs.MultitenancyAspire.Infrastructure.Common;
using Genocs.MultitenancyAspire.Infrastructure.Cors;
using Genocs.MultitenancyAspire.Infrastructure.FileStorage;
using Genocs.MultitenancyAspire.Infrastructure.Localization;
using Genocs.MultitenancyAspire.Infrastructure.Mailing;
using Genocs.MultitenancyAspire.Infrastructure.Mapping;
using Genocs.MultitenancyAspire.Infrastructure.Middleware;
using Genocs.MultitenancyAspire.Infrastructure.Multitenancy;
using Genocs.MultitenancyAspire.Infrastructure.Notifications;
using Genocs.MultitenancyAspire.Infrastructure.OpenApi;
using Genocs.MultitenancyAspire.Infrastructure.Persistence;
using Genocs.MultitenancyAspire.Infrastructure.Persistence.Initialization;
using Genocs.MultitenancyAspire.Infrastructure.SecurityHeaders;
using Genocs.MultitenancyAspire.Infrastructure.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Genocs.MultitenancyAspire.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(Application.Startup).GetTypeInfo().Assembly;
        MapsterSettings.Configure();
        return services
            .AddGnxApiVersioning()
            .AddGnxAuth(config)
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

    private static IServiceCollection AddGnxApiVersioning(this IServiceCollection services)
    {
        // Core API Versioning services with support for Minimal APIs
        // API version-aware extensions for MVC Core with controllers (not full MVC)
        services
                .AddApiVersioning()
                .AddMvc()
                .AddApiExplorer();

        // API version-aware API Explorer extensions
        services.AddEndpointsApiExplorer();
        return services;
    }

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
            .UseGnxCurrentUser()
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
        endpoints.MapHealthChecks("/healthz");
}