using Genocs.Core.Builders;
using Genocs.WebApi.Security;

namespace GenocsAspire.ProductsApiService;

public static class Extensions
{
    public static IGenocsBuilder AddServices(this IGenocsBuilder builder)
    {
        builder.AddCertificateAuthentication();

        // Add services to the container.
        builder.Services.AddProblemDetails();

        // Add here services like API client
        //       builder.Services.AddSingleton<IPricingServiceClient, PricingServiceClient>();
        return builder;
    }
}