using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetProducts();
    IEnumerable<Product> GetProductsWithCategory();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
}

