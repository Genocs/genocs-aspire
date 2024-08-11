using Genocs.Core.CQRS.Queries;
using GenocsAspire.ProductsApiService.DTO;

namespace GenocsAspire.ProductsApiService.Queries;

public class GetProduct : IQuery<ProductDto>
{
    public Guid ProductId { get; set; }
}