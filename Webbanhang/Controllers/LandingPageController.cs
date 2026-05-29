using Microsoft.AspNetCore.Mvc;
using Webbanhang.Repositories;

namespace Webbanhang.Controllers
{
    public class LandingPageController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public LandingPageController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Hiển thị trang Landing Page cao cấp
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách danh mục từ Database
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            // Gửi dữ liệu sang View
            ViewBag.Categories = categories;
            ViewBag.Products = products;
            ViewBag.FeaturedProducts = products.Take(4); // 4 sản phẩm nổi bật

            return View();
        }

        /// <summary>
        /// API endpoint - Lấy danh mục (JSON)
        /// </summary>
        [HttpGet("api/landing/categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Json(new { success = true, data = categories });
        }

        /// <summary>
        /// API endpoint - Lấy sản phẩm theo danh mục
        /// </summary>
        [HttpGet("api/landing/products/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var allProducts = await _productRepository.GetAllAsync();
            var filtered = allProducts.Where(p => p.CategoryId == categoryId).ToList();
            return Json(new { success = true, data = filtered });
        }

        /// <summary>
        /// Xử lý booking từ landing page
        /// </summary>
        [HttpPost("api/landing/book")]
        public IActionResult Book([FromBody] BookingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ" });
            }

            // TODO: Lưu booking vào database
            // Tạm thời chỉ return thành công
            return Json(new 
            { 
                success = true, 
                message = $"Cảm ơn {request.Name}! Chúng tôi sẽ liên hệ với bạn tại {request.Email}" 
            });
        }
    }

    /// <summary>
    /// Model cho yêu cầu booking
    /// </summary>
    public class BookingRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
