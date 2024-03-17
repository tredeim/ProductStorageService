using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Infrastructure;
using Domain;
using Api.Application;

namespace Api.Services
{
    public class ProductService : Api.ProductService.ProductServiceBase 
    {
        private IProductServiceApp _productService;
        public ProductService(IProductServiceApp productService)
        {
            _productService = productService;
        }

        public override Task<AddProductResponse> Add(AddProductRequest request, ServerCallContext context)
        {
            var dto = new ProductDto()
            {
                Name = request.Name,
                Price = request.Price,
                Weight = request.Weight,
                Type = (Domain.ProductType)request.Type,
                CreationDate = DateOnly.Parse(request.CreationDate),
                WarehouseId = request.WarehouseId
            };
            dto = _productService.Add(dto);

            return Task.FromResult(new AddProductResponse
            {
                Product = ProductExtensions.ToGrpcProduct(dto)
            });

           throw new RpcException(new Status(StatusCode.Internal, "Could not add the product"));
        }

        public override Task<GetProductResponse> Get(GetProductRequest request, ServerCallContext context)
        {
            var product = _productService.Get(request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID {request.Id} not found."));
            }

            return Task.FromResult(new GetProductResponse
            {
                Product = ProductExtensions.ToGrpcProduct(product)
            });
        }

        public override Task<UpdateProductPriceResponse> UpdatePrice(UpdateProductPriceRequest request, ServerCallContext context)
        {
            var product = _productService.UpdatePrice(request.Id, request.NewPrice);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product with ID {request.Id} not found."));
            }

            return Task.FromResult(new UpdateProductPriceResponse
            {
                Product = ProductExtensions.ToGrpcProduct(product)
            });
        }

        public override Task<ListProductResponse> List(ListProductRequest request, ServerCallContext context)
        {

            DateOnly? creationDate = string.IsNullOrEmpty(request.Date) ? null : DateOnly.Parse(request.Date);
            Domain.ProductType? type = request.Type == ProductType.Unknown ? null : (Domain.ProductType)request.Type;
            int? warehouseId = request.Warehouse == 0 ? null : request.Warehouse;
            

            var products = _productService.List(request.PageSize, request.Page, creationDate, type, warehouseId);

            return Task.FromResult(new ListProductResponse
            {
                Products = 
                {
                    products.Select(ProductExtensions.ToGrpcProduct)
                }
            });
           
        }
    }
}
