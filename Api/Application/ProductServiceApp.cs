using Infrastructure;

namespace Api.Application
{
    public class ProductServiceApp : IProductServiceApp
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceApp(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ProductDto Add(ProductDto productDto)
        {
            var product = new Domain.Product
            {
                Name = productDto.Name, 
                Price = productDto.Price,
                Weight = productDto.Weight,
                Type = productDto.Type,
                CreationDate = productDto.CreationDate,  
                WarehouseId = productDto.WarehouseId
            };

            product = _productRepository.Add(product);
            

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                Type = product.Type,
                CreationDate = product.CreationDate,
                WarehouseId = product.WarehouseId
            };
        }

        public ProductDto Get(int productId)
        {
            var product = _productRepository.Get(productId);
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                Type = product.Type,
                CreationDate = product.CreationDate,
                WarehouseId = product.WarehouseId
            };
        }

        public ProductDto UpdatePrice(int productId, double newPrice)
        {
            var product = _productRepository.UpdatePrice(productId, newPrice);
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Weight = product.Weight,
                Type = product.Type,
                CreationDate = product.CreationDate,
                WarehouseId = product.WarehouseId
            };
        }


        public List<ProductDto> List(int pageSize, int page, DateOnly? date, Domain.ProductType? type, int? warehouse)
        {
            return _productRepository.List(pageSize, page, date, type, warehouse)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Weight = p.Weight,
                    Type = p.Type,
                    CreationDate = p.CreationDate,
                    WarehouseId = p.WarehouseId
                })
                .ToList();
        }
    }
}
