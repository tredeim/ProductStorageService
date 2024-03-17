using Api.Application;
using System.Reflection;
using Domain;

namespace Api.Services
{
    public static class ProductExtensions
    {
        public static Product ToGrpcProduct(ProductDto product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                Type = (ProductType) product.Type,
                CreationDate = product.CreationDate.ToString("o"), // ISO 8601
                WarehouseId = product.WarehouseId
            };
        }
    }
}
