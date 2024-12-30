using Genocs.MultitenancyAspire.Application.Catalog.Brands;

namespace Genocs.MultitenancyAspire.Application.Catalog.Products;

public class ProductDetailsDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public string? ImagePath { get; set; }
    public BrandDto Brand { get; set; } = default!;
}