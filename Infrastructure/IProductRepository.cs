using Domain;

namespace Infrastructure
{
    public interface IProductRepository
    {
        Product Add(Product product);
        Product Get(int productId);
        Product UpdatePrice(int productId, double newPrice);
        List<Product> List(int pageSize, int page, DateOnly? date, ProductType? type, int? warehouse);
    }
}