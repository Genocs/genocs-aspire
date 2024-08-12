namespace GenocsAspire.Multitenancy.Application.Catalog.Products;

public class ProductByIdWithBrandSpec : Specification<Product, ProductDetailsDto>, ISingleResultSpecification<Product, ProductDetailsDto>
{
    public ProductByIdWithBrandSpec(DefaultIdType id) =>
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand);
}