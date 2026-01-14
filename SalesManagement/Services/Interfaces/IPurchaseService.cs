using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface IPurchaseService
{
    void AddPurchase(Purchase purchase);
    void UpdatePurchase(Purchase purchase);
    IEnumerable<Purchase> GetPurchases();
    Purchase? GetPurchaseById(int id);
}
