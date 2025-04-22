using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Category { get; set; }

        public double? UnitPrice { get; set; }

        public int? QuantityInStock { get; set; }
    }
}
