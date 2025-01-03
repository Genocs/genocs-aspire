using Genocs.Logging;
using Genocs.Core;
using Genocs.MultitenancyAspire.Application;
using Genocs.MultitenancyAspire.Infrastructure;
using Genocs.MultitenancyAspire.Infrastructure.Logging.Serilog;
using Genocs.MultitenancyAspire.WebApi.Configurations;
using Genocs.MultitenancyAspire.WebApi.Controllers;
using Serilog;
using Genocs.Core.Builders;

[assembly: ApiConventionType(typeof(GNXApiConventions))]

StaticLogger.EnsureInitialized();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations();

    builder
            .AddGenocs()
            .Build();

    builder.Host
        .UseLogging(builder.Environment.EnvironmentName);

    builder.Services.AddControllers();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}