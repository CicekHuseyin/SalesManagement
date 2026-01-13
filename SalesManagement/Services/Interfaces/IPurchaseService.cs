using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface IPurchaseService
{
    void AddPurchase(Purchase purchase);
    IEnumerable<Purchase> GetPurchases();
}
