using System.ComponentModel.DataAnnotations;

namespace StoreApp.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name is required and cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }
}
