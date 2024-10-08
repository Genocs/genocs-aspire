using GenocsAspire.Multitenancy.Application;
using GenocsAspire.Multitenancy.Infrastructure;
using GenocsAspire.Multitenancy.Infrastructure.Common;
using GenocsAspire.Multitenancy.Infrastructure.Logging.Serilog;
using GenocsAspire.Multitenancy.WebApi.Configurations;
using GenocsAspire.Multitenancy.WebApi.Controllers;
using Serilog;

[assembly: ApiConventionType(typeof(GNXApiConventions))]

StaticLogger.EnsureInitialized();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder
        .SetBanner()
        .AddConfigurations();

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
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}