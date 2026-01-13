using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface ISaleService
{
    void AddSale(Sale sale);
    IEnumerable<Sale> GetSales();
}
