var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var messaging = builder.AddRabbitMQ("messaging");

var apiService = builder.AddProject<Projects.GenocsAspire_ApiService>("apiservice");
var apiGatewayService = builder.AddProject<Projects.GenocsAspire_APIGateway>("apiGateway");
var identityService = builder.AddProject<Projects.GenocsAspire_IdentitiesApiService>("identities");
var signalRService = builder.AddProject<Projects.GenocsAspire_SignalRApiService>("signalR");
var productsService = builder.AddProject<Projects.GenocsAspire_ProductsApiService>("products");
var orderssService = builder.AddProject<Projects.GenocsAspire_OrdersApiService>("orders");
var multitenancyService = builder.AddProject<Projects.GenocsAspire_Multitenancy_WebApi>("multitenancy");


builder.AddProject<Projects.GenocsAspire_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(messaging)
    .WithReference(signalRService)
    .WithReference(apiService);

builder.Build().Run();
