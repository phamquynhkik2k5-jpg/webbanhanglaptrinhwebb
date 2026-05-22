using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Webbanhang.Models;
using Webbanhang.Repositories;

namespace Webbanhang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // =========================
        // Add Product - GET
        // =========================
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();

            ViewBag.Categories = new SelectList(
                categories,
                "Id",
                "Name"
            );

            return View();
        }

        // =========================
        // Add Product - POST
        // =========================
        [HttpPost]
        public async Task<IActionResult> Add(
            Product product,
            IFormFile imageUrl,
            List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                // Lưu ảnh đại diện
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                // Lưu nhiều ảnh khác
                if (imageUrls != null)
                {
                    product.ImageUrls = new List<string>();

                    foreach (var file in imageUrls)
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }

                _productRepository.Add(product);

                return RedirectToAction("Index");
            }

            var categories = _categoryRepository.GetAllCategories();

            ViewBag.Categories = new SelectList(
                categories,
                "Id",
                "Name"
            );

            return View(product);
        }

        // =========================
        // Save Image
        // =========================
        private async Task<string> SaveImage(IFormFile image)
        {
            // Đường dẫn thư mục lưu ảnh
            var savePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images"
            );

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // Tạo tên file tránh trùng
            var fileName = Guid.NewGuid().ToString()
                           + Path.GetExtension(image.FileName);

            // Đường dẫn đầy đủ
            var filePath = Path.Combine(savePath, fileName);

            // Lưu file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn lưu DB
            return "/images/" + fileName;
        }

        // =========================
        // Display a list of products
        // =========================
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        // =========================
        // Display a single product
        // =========================
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // =========================
        // Show the product update form
        // =========================
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // =========================
        // Process the product update
        // =========================
        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // =========================
        // Show the product delete confirmation
        // =========================
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // =========================
        // Process the product deletion
        // =========================
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}