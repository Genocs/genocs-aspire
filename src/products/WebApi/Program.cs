using Genocs.Core.Builders;
using Genocs.Core.CQRS.Commands;
using Genocs.Core.CQRS.Events;
using Genocs.Core.CQRS.Queries;
using Genocs.Discovery.Consul;
using Genocs.HTTP;
using Genocs.LoadBalancing.Fabio;
using Genocs.Logging;
using Genocs.MessageBrokers.Outbox;
using Genocs.MessageBrokers.Outbox.MongoDB;
using Genocs.MessageBrokers.RabbitMQ;
using Genocs.Metrics.AppMetrics;
using Genocs.Metrics.Prometheus;
using Genocs.Persistence.MongoDb.Extensions;
using Genocs.Persistence.Redis;
using Genocs.Secrets.Vault;
using Genocs.Tracing;
using Genocs.Tracing.Jaeger;
using Genocs.Tracing.Jaeger.RabbitMQ;
using Genocs.WebApi;
using Genocs.WebApi.CQRS;
using Genocs.WebApi.Security;
using Genocs.WebApi.Swagger;
using Genocs.WebApi.Swagger.Docs;
using GenocsAspire.ProductsApiService;
using GenocsAspire.ProductsApiService.Commands;
using GenocsAspire.ProductsApiService.Domain;
using GenocsAspire.ProductsApiService.DTO;
using GenocsAspire.ProductsApiService.Queries;
using GenocsAspire.ServiceDefaults;
using Serilog;

StaticLogger.EnsureInitialized();

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Host
        .UseLogging()
        .UseVault();

var services = builder.Services;

services.AddGenocs()
        .AddErrorHandler<ExceptionToResponseMapper>()
        .AddServices()
        .AddHttpClient()
        .AddCorrelationContextLogging()
        .AddConsul()
        .AddFabio()
        .AddOpenTelemetry()
        .AddJaeger()
        .AddMetrics()
        .AddMongo()
        .AddMongoRepository<Product, Guid>("products")
        .AddCommandHandlers()
        .AddEventHandlers()
        .AddQueryHandlers()
        .AddInMemoryCommandDispatcher()
        .AddInMemoryEventDispatcher()
        .AddInMemoryQueryDispatcher()
        .AddPrometheus()
        .AddRedis()
        .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
        .AddMessageOutbox(o => o.AddMongo())
        .AddWebApi()
        .AddSwaggerDocs()
        .AddWebApiSwaggerDocs()
        .Build();

var app = builder.Build();

app.UseGenocs()
    .UserCorrelationContextLogging()
    .UseErrorHandler()
    .UsePrometheus()
    .UseRouting()
    .UseCertificateAuthentication()
    .UseEndpoints(r => r.MapControllers())
    .UseDispatcherEndpoints(endpoints => endpoints
        .Get(string.Empty, ctx => ctx.Response.WriteAsync("Products Service"))
        .Get("ping", ctx => ctx.Response.WriteAsync("pong"))
        .Get<BrowseProducts, PagedResult<ProductDto>>("products")
        .Get<GetProduct, ProductDto>("products/{productId}")
        .Post<CreateProduct>("products", afterDispatch: (cmd, ctx) => ctx.Response.Created($"products/{cmd.ProductId}")))
    .UseJaeger()
    .UseSwaggerDocs()
    .UseRabbitMq();

app.MapDefaultEndpoints();

app.Run();

Log.CloseAndFlush();