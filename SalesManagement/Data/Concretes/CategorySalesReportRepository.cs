using Microsoft.EntityFrameworkCore;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;

namespace SalesManagement.Data.Concretes;

public class CategorySalesReportRepository : ICategorySalesReportRepository
{
    private readonly TestDbContext _context;

    public CategorySalesReportRepository(TestDbContext context)
    {
        _context = context;
    }

    public IEnumerable<CategorySalesReportDto> GetAllSalesDetail()
    {
        // Stored procedure çağrısı
        return _context.CategorySalesReportDtos
            .FromSqlRaw("EXEC SPReportGetAllSalesDetail")
            .AsEnumerable() // LINQ işlemlerini client tarafında yap
            .ToList();
    }
}