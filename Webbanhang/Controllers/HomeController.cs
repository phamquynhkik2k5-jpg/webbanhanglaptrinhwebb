using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public IActionResult Index()
        {
            // Lấy 6 sản phẩm mới nhất để hiển thị trên trang chủ
            var products = _productRepository.GetAll()
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
