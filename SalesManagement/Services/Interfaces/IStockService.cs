using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface IStockService
{
    void IncreaseStock(int productId, double quantity);
    void DecreaseStock(int productId, double quantity);
    double GetCurrentStock(int productId);
}
