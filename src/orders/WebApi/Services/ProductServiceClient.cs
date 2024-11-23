using Genocs.HTTP;
using Genocs.HTTP.Configurations;
using Genocs.Secrets.Vault.Configurations;
using Genocs.WebApi.Security.Configurations;
using GenocsAspire.OrdersApiService.DTO;

namespace GenocsAspire.OrdersApiService.Services;

/// <summary>
/// The Product WebApi client implementation.
/// </summary>
public class ProductServiceClient : IProductServiceClient
{
    private readonly IHttpClient _client;
    private readonly string _url;

    /// <summary>
    /// The standard constructor.
    /// </summary>
    /// <param name="client">The http client.</param>
    /// <param name="httpClientOptions"></param>
    public ProductServiceClient(
                                IHttpClient client,
                                HttpClientOptions httpClientOptions)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));


        if (httpClientOptions is null)
        {
            throw new ArgumentNullException(nameof(httpClientOptions));
        }

        string? url = httpClientOptions?.Services?["products"];

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new Exception("products http client option cannot be null");
        }

        _url = url;
    }

    /// <summary>
    /// Get the product result based on the productId.
    /// </summary>
    /// <param name="productId">The ProductId.</param>
    /// <returns>The Product Response.</returns>
    public Task<ProductDto> GetAsync(Guid productId)
        => _client.GetAsync<ProductDto>($"{_url}/products/{productId}");
}