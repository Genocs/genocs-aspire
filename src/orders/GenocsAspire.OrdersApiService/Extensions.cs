using Genocs.Core.Builders;
using Genocs.WebApi.Security;
using GenocsAspire.OrdersApiService.Services;

namespace GenocsAspire.OrdersApiService;

public static class Extensions
{
    public static IGenocsBuilder AddServices(this IGenocsBuilder builder)
    {
        builder.AddCertificateAuthentication();
        builder.Services.AddSingleton<IProductServiceClient, ProductServiceClient>();
        return builder;
    }
}