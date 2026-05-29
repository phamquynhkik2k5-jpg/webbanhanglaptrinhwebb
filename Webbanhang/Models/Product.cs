using System.ComponentModel.DataAnnotations;

namespace Webbanhang.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty; // Gán giá trị mặc định để fix cảnh báo CS8618

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; } // Ảnh đại diện

        public List<ProductImage>? Images { get; set; } // Danh sách ảnh phụ

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}