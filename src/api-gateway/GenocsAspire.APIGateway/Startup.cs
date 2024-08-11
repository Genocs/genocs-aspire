using Genocs.APIGateway.Configurations;
using Genocs.APIGateway.Framework;
using Genocs.Auth;
using Genocs.Common.Configurations;
using Genocs.Core.Builders;
using Genocs.MessageBrokers.RabbitMQ;
using Genocs.Metrics.Prometheus;
using Genocs.Security;
using Genocs.Tracing;
using Genocs.Tracing.Jaeger;
using Genocs.Tracing.Jaeger.RabbitMQ;
using Genocs.WebApi;
using Yarp.ReverseProxy.Forwarder;

namespace Genocs.APIGateway;

internal class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddProblemDetails();

        services.AddScoped<LogContextMiddleware>();
        services.AddScoped<UserMiddleware>();
        services.AddScoped<MessagingMiddleware>();
        services.AddSingleton<CorrelationIdFactory>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<ICorrelationContextBuilder, CorrelationContextBuilder>();
        services.AddSingleton<RouteMatcher>();
        services.Configure<MessagingOptions>(Configuration.GetSection(MessagingOptions.Position));
        services.AddReverseProxy()
            .LoadFromConfig(Configuration.GetSection("ReverseProxy"));
        services.AddSingleton<IForwarderHttpClientFactory, CustomForwarderHttpClientFactory>();
        services
            .AddGenocs()
            .AddOpenTelemetry()
            .AddJaeger()
            .AddJwt()
            .AddPrometheus()
            .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
            .AddSecurity()
            .AddWebApi()
            .Build();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("authenticatedUser", policy =>
                policy.RequireAuthenticatedUser());
        });

        services.AddCors(cors =>
        {
            cors.AddPolicy("cors", x =>
            {
                x.WithOrigins("*")
                    .WithMethods("POST", "PUT", "DELETE")
                    .WithHeaders("Content-Type", "Authorization");
            });
        });

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<LogContextMiddleware>();
        app.UseCors("cors");
        app.UseGenocs();
        app.UseJaeger();
        app.UsePrometheus();
        app.UseAccessTokenValidator();
        app.UseAuthentication();
        app.UseRabbitMq();
        app.UseMiddleware<UserMiddleware>();
        app.UseMiddleware<MessagingMiddleware>();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync(context.RequestServices.GetService<AppOptions>()?.Name ?? "Service");
            });
            endpoints.MapReverseProxy();

            endpoints.MapHealthChecks("/hc");
        });
    }
}
