using Genocs.Core.CQRS.Queries;
using GenocsAspire.ProductsApiService.DTO;

namespace GenocsAspire.ProductsApiService.Queries;

public class BrowseProducts : PagedQueryBase, IQuery<PagedResult<ProductDto>>
{

}