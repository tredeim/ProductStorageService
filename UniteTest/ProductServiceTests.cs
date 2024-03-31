using Api.Application;
using Infrastructure;
using Moq;

namespace UniteTest
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductServiceApp _service;
        private readonly ProductDto _testProductDto;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new ProductServiceApp(_mockRepository.Object);
            _testProductDto = new ProductDtoBuilder().Build(); 
        }

        [Fact]
        public void Add_ShouldAddProduct_WhenProductIsValid()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Add(It.IsAny<Domain.Product>()))
                           .Returns(new Domain.Product { Id = 1, Name = "Test Product", Price = 100 });

            // Act
            var result = _service.Add(_testProductDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Product", result.Name);
            _mockRepository.Verify(repo => repo.Add(It.IsAny<Domain.Product>()), Times.Once);
        }

        [Fact]
        public void Get_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            _mockRepository.Setup(repo => repo.Get(productId))
                           .Returns(new Domain.Product { Id = productId, Name = "Test Product", Price = 100 });

            // Act
            var result = _service.Get(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            _mockRepository.Verify(repo => repo.Get(productId), Times.Once);
        }

        [Fact]
        public void UpdatePrice_ShouldUpdateProductPrice_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var newPrice = 150;
            _mockRepository.Setup(repo => repo.UpdatePrice(productId, newPrice))
                           .Returns(new Domain.Product { Id = productId, Name = "Test Product", Price = newPrice });

            // Act
            var result = _service.UpdatePrice(productId, newPrice);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newPrice, result.Price);
            _mockRepository.Verify(repo => repo.UpdatePrice(productId, newPrice), Times.Once);
        }


        [Fact]
        public void List_ShouldReturnProducts_WhenCalledWithValidParameters()
        {
            // Arrange
            var pageSize = 10;
            var page = 1;
            _mockRepository.Setup(repo => repo.List(pageSize, page, null, null, null))
                           .Returns(new List<Domain.Product> { new Domain.Product { Id = 1, Name = "Test Product", Price = 100 } });

            // Act
            var result = _service.List(pageSize, page, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepository.Verify(repo => repo.List(pageSize, page, null, null, null), Times.Once);
        }

    }
}