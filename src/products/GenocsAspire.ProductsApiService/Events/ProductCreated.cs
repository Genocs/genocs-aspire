using Genocs.Core.CQRS.Events;

namespace GenocsAspire.ProductsApiService.Events;

public class ProductCreated : IEvent
{
    public Guid ProductId { get; }

    public ProductCreated(Guid productId)
    {
        ProductId = productId;
    }
}