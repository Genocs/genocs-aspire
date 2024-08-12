using GenocsAspire.Multitenancy.Application.Common.Interfaces;

namespace GenocsAspire.Multitenancy.Application.Catalog.Brands;

public class BrandDto : IDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}