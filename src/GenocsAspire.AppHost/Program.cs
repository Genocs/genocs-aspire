var builder = DistributedApplication.CreateBuilder(args);

var apiGatewayService = builder.AddProject<Projects.APIGateway_WebApi>("apiGateway");
var identityService = builder.AddProject<Projects.Identities_WebApi>("identities");
var productsService = builder.AddProject<Projects.Products_WebApi>("products");
var ordersService = builder.AddProject<Projects.Orders_WebApi>("orders");
var signalRService = builder.AddProject<Projects.SignalR_WebApi>("signalR");
var multitenancyService = builder.AddProject<Projects.Multitenancy_WebApi>("multitenancy");

builder.AddProject<Projects.Host>("blazorFrontend")
    .WithExternalHttpEndpoints()
    .WithReference(multitenancyService);

builder.Build().Run();
