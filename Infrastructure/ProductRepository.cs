using Domain;
using System.Collections.Concurrent;


namespace Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private ConcurrentDictionary<int, Product> _storage;
        private int _currentId =  0;

        public ProductRepository()
        {
            _storage = new ConcurrentDictionary<int, Product>();
        } 

        public Product Add(Product product)
        {
            var productId = Interlocked.Increment(ref _currentId);
            _storage.TryAdd(productId, product);
            product.Id = productId;

            return product;
        }

        public Product? Get(int productId)
        {
            if (_storage.TryGetValue(productId, out var product))
            {
                return product;
            }

            return null;
        }

        public Product? UpdatePrice(int productId, double newPrice)
        {
            var product = Get(productId);
            if (product != null)
            {
                product.Price = newPrice;
                return product;
            }

            return null;
        }

        public List<Product> List(int pageSize, int page, DateOnly? date , ProductType? type, int? warehouse)
        {
           
            var filteredStorage = _storage.Values.AsEnumerable();
            if (date.HasValue)
            {
                filteredStorage = filteredStorage.Where(p => p.CreationDate == date);
            }
            if (type.HasValue)
            {
                filteredStorage = filteredStorage.Where(p => p.Type == type);
            }
            if (warehouse.HasValue)
            {
                filteredStorage = filteredStorage.Where(p => p.WarehouseId == warehouse);
            }

            return filteredStorage
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToList();
        }
    }
}
