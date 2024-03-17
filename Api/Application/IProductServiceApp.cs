namespace Api.Application
{
    public interface IProductServiceApp
    {
        ProductDto Add(ProductDto product);
        ProductDto Get(int productId);
        ProductDto UpdatePrice(int productId, double newPrice);
        List<ProductDto> List(int pageSize, int page, DateOnly? date, Domain.ProductType? type, int? warehouse);
    }
}
