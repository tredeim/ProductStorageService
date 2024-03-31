using Api.Application;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using Moq;
using Domain;


namespace IntegrationTests
{
    public class ProductServiceTests : IClassFixture<MyCustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly MyCustomWebApplicationFactory<Program> _factory;

        public ProductServiceTests(MyCustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddProduct_ReturnsNewProduct()
        {
            // Arrange
            var newProduct = new ProductDto { Name = "New Product", Price = 9.99, Weight = 0.5, Type = ProductType.GENERAL, CreationDate = new DateOnly(2024, 1, 1), WarehouseId = 1 };
            _factory.ProductRepositoryFake.Setup(repo => repo.Add(It.IsAny<Product>()))
                .Returns(new Product { Id = 3, Name = "New Product", Price = 9.99, Weight = 0.5, Type = Domain.ProductType.GENERAL, CreationDate = new DateOnly(2024, 1, 1), WarehouseId = 1 });

            // Act
            var response = await _client.PostAsJsonAsync("/v1/product/add", newProduct);

            // Assert
            response.EnsureSuccessStatusCode();
            var addedProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(addedProduct);
            Assert.Equal("New Product", addedProduct.Name);
            Assert.Equal(9.99, addedProduct.Price);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct()
        {
            // Arrange
            var expectedProduct = new Product { Id = 1, Name = "Existing Product", Price = 19.99, Weight = 0.7, Type = Domain.ProductType.GENERAL, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 2 };
            _factory.ProductRepositoryFake.Setup(repo => repo.Get(1))
                .Returns(expectedProduct);

            // Act
            var response = await _client.GetAsync("/v1/product/get");

            // Assert
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(product);
            Assert.Equal("Existing Product", product.Name);
            Assert.Equal(19.99, product.Price);
        }

        [Fact]
        public async Task UpdateProductPrice_ReturnsUpdatedProduct()
        {
            // Arrange
            var productId = 1;
            var newPrice = 200.0;
            _factory.ProductRepositoryFake.Setup(repo => repo.UpdatePrice(productId, newPrice))
                .Returns(new Product { Id = productId, Name = "Updated Product", Price = newPrice, Weight = 10, Type = ProductType.APPLIANCES, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 1 });

            // Act
            var response = await _client.PutAsJsonAsync($"/v1/product/updatePrice", new { newPrice });

            // Assert
            response.EnsureSuccessStatusCode();
            var updatedProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(updatedProduct);
            Assert.Equal(productId, updatedProduct.Id);
            Assert.Equal("Updated Product", updatedProduct.Name);
            Assert.Equal(newPrice, updatedProduct.Price);
        }

        [Fact]
        public async Task ListProducts_ReturnsProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100, Weight = 10, Type = ProductType.GENERAL, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 150, Weight = 20, Type = ProductType.FOOD, CreationDate = DateOnly.FromDateTime(DateTime.Now), WarehouseId = 2 }
            };
            _factory.ProductRepositoryFake.Setup(repo => repo.List(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateOnly?>(), It.IsAny<ProductType?>(), It.IsAny<int?>()))
                .Returns(expectedProducts);
            // Act
            var response = await _client.GetAsync("/v1/product/list");

            // Assert
            response.EnsureSuccessStatusCode();
            var productsList = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            Assert.NotNull(productsList);
            Assert.Equal(expectedProducts.Count, productsList.Count);
            Assert.Contains(productsList, p => p.Name == "Product 1"); 
            Assert.Contains(productsList, p => p.Price == 150);
        }
    }
}