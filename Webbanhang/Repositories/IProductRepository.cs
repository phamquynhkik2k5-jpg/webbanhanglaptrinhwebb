using System.Collections.Generic;
using System.Threading.Tasks;
using Webbanhang.Models;

namespace Webbanhang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        // Sử dụng Product? (nullable) để tránh cảnh báo nếu không tìm thấy sản phẩm
        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    }
}