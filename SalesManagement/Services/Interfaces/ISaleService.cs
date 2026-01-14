using SalesManagement.Models;

namespace SalesManagement.Services.Interfaces;

public interface ISaleService
{
    void AddSale(Sale sale);
    void UpdateSale(Sale sale);
    IEnumerable<Sale> GetSales();
    List<Sale> GetSalesWithProductAndCustomer();
    Sale? GetSaleById(int id);
}
