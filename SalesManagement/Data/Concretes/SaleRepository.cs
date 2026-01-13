using Microsoft.EntityFrameworkCore;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;

namespace SalesManagement.Data.Concretes;

public class SaleRepository : GenericRepository<Sale>, ISaleRepository
{
    private readonly TestDbContext _context;

    public SaleRepository(TestDbContext context) : base(context)
    {
        _context = context;
    }

    public List<Sale> GetSalesWithProductAndCustomer()
    {
        return _context.Sales
            .Include(x => x.Product)
            .Include(x => x.Customer)
            .ToList();
    }
}

