using SalesManagement.Models;

namespace SalesManagement.Data.Interfaces;

public interface ISaleRepository : IGenericRepository<Sale>
{
    List<Sale> GetSalesWithProductAndCustomer();
}