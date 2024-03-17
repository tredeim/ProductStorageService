namespace Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public double Price { get; set; }
        public double Weight { get; init; }
        public ProductType Type { get; init; }
        public DateOnly CreationDate { get; init; }
        public int WarehouseId { get; init; }
    }
}
