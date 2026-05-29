using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Webbanhang.Models;
using Webbanhang.Repositories;

namespace Webbanhang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        // Chuyển hàm Index thành async Task<IActionResult>
        public async Task<IActionResult> Index()
        {
            // 1. Đợi lấy toàn bộ danh sách sản phẩm từ Repository một cách bất đồng bộ
            var allProducts = await _productRepository.GetAllAsync();

            // 2. Sử dụng LINQ trên bộ nhớ để lọc ra 6 sản phẩm mới nhất
            var products = allProducts
                .OrderByDescending(p => p.Id)
                .Take(6)
                .ToList();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}