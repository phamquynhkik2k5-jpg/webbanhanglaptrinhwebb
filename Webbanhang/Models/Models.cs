using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Webbanhang.Models
{
    // Product.cs
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; } // Đường dẫn đến hình ảnh đại diện
        public List<string>? ImageUrls { get; set; } // Danh sách các hình ảnh
 
    }
    // Category.cs
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
