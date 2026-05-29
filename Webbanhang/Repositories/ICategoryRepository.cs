using System.Collections.Generic;
using System.Threading.Tasks;
using Webbanhang.Models;

namespace Webbanhang.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();

        // Sử dụng Category? (nullable) để tránh cảnh báo nếu không tìm thấy danh mục
        Task<Category?> GetByIdAsync(int id);

        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(int id);
    }
}