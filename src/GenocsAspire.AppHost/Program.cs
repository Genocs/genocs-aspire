var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var messaging = builder.AddRabbitMQ("messaging");

var apiGatewayService = builder.AddProject<Projects.APIGateway>("apiGateway");
var identityService = builder.AddProject<Projects.Identities_WebApi>("identities");
var signalRService = builder.AddProject<Projects.SignalR_WebApi>("signalR");
var productsService = builder.AddProject<Projects.Products_WebApi>("products");
var ordersService = builder.AddProject<Projects.Orders_WebApi>("orders");
var multitenancyService = builder.AddProject<Projects.Multitenancy_WebApi>("multitenancy");

builder.AddProject<Projects.Host>("blazorFrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(messaging)
    .WithReference(multitenancyService);

builder.Build().Run();
