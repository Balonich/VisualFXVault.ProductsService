namespace DataAccessLayer.Entities;

public class Product
{
    public Guid ProductID { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double? UnitPrice { get; set; }
    public int? QuantityInStock { get; set; }
}