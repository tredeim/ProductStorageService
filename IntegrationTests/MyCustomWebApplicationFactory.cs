using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Domain;


namespace IntegrationTests
{
    public class MyCustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        public readonly string Id = Guid.NewGuid().ToString();

        public readonly Mock<IProductRepository> ProductRepositoryFake = new();

        public MyCustomWebApplicationFactory()
        {
            ProductRepositoryFake
            .Setup(repo => repo.Get(1))
            .Returns(new Product { 
                Id = 1, 
                Name = "Test Product", 
                Price = 100.00, 
                Weight = 10, 
                Type = ProductType.GENERAL, 
                CreationDate = DateOnly.FromDateTime(DateTime.Now), 
                WarehouseId = 1 
            });

            ProductRepositoryFake
            .Setup(repo => repo.Get(It.Is<int>(id => id != 1)))
            .Returns((Product)null);


            ProductRepositoryFake
            .Setup(repo => repo.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateOnly?>(), It.IsAny<Domain.ProductType?>(), It.IsAny<int?>()))
            .Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Test Product 1", Price = 100.00, Weight = 10, Type = ProductType.GENERAL, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 1 },
                new Product { Id = 2, Name = "Test Product 2", Price = 200.00, Weight = 20, Type = ProductType.FOOD, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 2 }
            });

            ProductRepositoryFake
            .Setup(repo => repo.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateOnly?>(), It.Is<ProductType?>(type => type == ProductType.UNKNOWN), It.IsAny<int?>()))
            .Returns(new List<Product>());


            ProductRepositoryFake
            .Setup(repo => repo.UpdatePrice(1, It.IsAny<double>()))
            .Returns((int id, double newPrice) =>
            new Product { Id = 1, Name = "Test Product", Price = newPrice, Weight = 10, Type = Domain.ProductType.GENERAL, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 1 });

           
            ProductRepositoryFake
            .Setup(repo => repo.UpdatePrice(It.Is<int>(id => id != 1), It.IsAny<double>()))
            .Returns((Product)null);


            ProductRepositoryFake
                .Setup(repo => repo.Add(It.IsAny<Product>()))
                .Returns((Product product) => {
                    product.Id = 3;
                    return product;
                });

            
            ProductRepositoryFake
            .Setup(repo => repo.Add(It.Is<Product>(product => string.IsNullOrEmpty(product.Name) || product.Price <= 0)))
            .Returns((Product)null);
        }



        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(new ServiceDescriptor(typeof(IProductRepository), ProductRepositoryFake.Object));
            });
        }
    }

}
