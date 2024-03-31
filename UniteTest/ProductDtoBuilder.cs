using Api.Application;
using Domain;

public class ProductDtoBuilder
{
    private int _id = 1;
    private string _name = "Default Product";
    private double _price = 100.0;
    private double _weight = 1.0;
    private ProductType _type = ProductType.UNKNOWN;
    private DateOnly _creationDate = DateOnly.FromDateTime(DateTime.Now);
    private int _warehouseId = 1;

    public ProductDtoBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductDtoBuilder WithPrice(double price)
    {
        _price = price;
        return this;
    }

    public ProductDtoBuilder WithWeight(double weight)
    {
        _weight = weight;
        return this;
    }

    public ProductDtoBuilder WithType(ProductType type)
    {
        _type = type;
        return this;
    }

    public ProductDtoBuilder WithCreationDate(DateOnly creationDate)
    {
        _creationDate = creationDate;
        return this;
    }

    public ProductDtoBuilder WithWarehouseId(int warehouseId)
    {
        _warehouseId = warehouseId;
        return this;
    }

    public ProductDto Build()
    {
        return new ProductDto
        {
            Id = _id,
            Name = _name,
            Price = _price,
            Weight = _weight,
            Type = _type,
            CreationDate = _creationDate,
            WarehouseId = _warehouseId
        };
    }
}
