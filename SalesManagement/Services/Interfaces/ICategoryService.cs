using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface ICategoryService
{
    IEnumerable<Category> GetCategories();
    void AddCategory(Category category);
    void UpdateCategory(Category category);
}
