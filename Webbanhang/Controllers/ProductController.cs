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

        private static readonly string[] AllowedImageExtensions =
            { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        private const long MaxFileSize = 2 * 1024 * 1024; // 2MB

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(
            Product product,
            IFormFile imageUrl,
            List<IFormFile> imageUrls)
        {
            ValidateImageFile(imageUrl, "imageUrl");

            if (imageUrls != null)
            {
                for (int i = 0; i < imageUrls.Count; i++)
                {
                    ValidateImageFile(imageUrls[i], $"imageUrls[{i}]");
                }
            }

            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                if (imageUrls != null && imageUrls.Any(f => f != null))
                {
                    product.ImageUrls = new List<string>();

                    foreach (var file in imageUrls.Where(f => f != null))
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }

                _productRepository.Add(product);
                return RedirectToAction("Index");
            }

            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        private void ValidateImageFile(IFormFile file, string key)
        {
            if (file == null) return;

            if (file.Length == 0)
            {
                ModelState.AddModelError(key, "Tệp ảnh rỗng.");
                return;
            }

            if (file.Length > MaxFileSize)
            {
                ModelState.AddModelError(key, "Kích thước tệp tối đa là 2MB.");
            }

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(ext) || !AllowedImageExtensions.Contains(ext))
            {
                ModelState.AddModelError(key, "Chỉ chấp nhận JPG, JPEG, PNG, GIF, WEBP.");
            }
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName).ToLowerInvariant()}";
            var filePath = Path.Combine(savePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + fileName;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            Product product,
            IFormFile imageUrl,
            List<IFormFile> imageUrls)
        {
            var oldProduct = _productRepository.GetById(product.Id);
            if (oldProduct == null) return NotFound();

            ValidateImageFile(imageUrl, "imageUrl");

            if (imageUrls != null)
            {
                for (int i = 0; i < imageUrls.Count; i++)
                {
                    ValidateImageFile(imageUrls[i], $"imageUrls[{i}]");
                }
            }

            if (ModelState.IsValid)
            {
                // Giữ ảnh cũ nếu không upload ảnh mới
                product.ImageUrl = oldProduct.ImageUrl;
                product.ImageUrls = oldProduct.ImageUrls ?? new List<string>();

                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                if (imageUrls != null && imageUrls.Any(f => f != null))
                {
                    foreach (var file in imageUrls.Where(f => f != null))
                    {
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }

                _productRepository.Update(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}