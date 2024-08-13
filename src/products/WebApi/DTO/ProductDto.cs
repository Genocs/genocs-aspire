namespace GenocsAspire.ProductsApiService.DTO;

public class ProductDto
{
    public Guid Id { get; set; }
    public string SKU { get; set; } = default!;

    public decimal UnitPrice { get; set; }
}