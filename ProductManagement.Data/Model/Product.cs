using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Data
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}