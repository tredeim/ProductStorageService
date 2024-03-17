namespace Api.Application
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public Domain.ProductType Type { get; set; }
        public DateOnly CreationDate { get; set; }
        public int WarehouseId { get; set; }
    }
}
