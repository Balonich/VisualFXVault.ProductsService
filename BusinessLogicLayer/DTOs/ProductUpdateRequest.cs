namespace BusinessLogicLayer.DTOs
{
    public class ProductUpdateRequest
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Category { get; set; }
        public double? UnitPrice { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
