using SalesManagement.Data.Interfaces;
using SalesManagement.Services.Interfaces;
using SalesManagement.ViewModels.Report;

namespace SalesManagement.Services.Concretes;

public class CategorySalesReportService : ICategorySalesReportService
{
    private readonly ICategorySalesReportRepository _repo;

    public CategorySalesReportService(ICategorySalesReportRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<CategorySalesReportViewModel> GetCategorySalesReport()
    {
        return _repo.GetAllSalesDetail()
            .Select(x => new CategorySalesReportViewModel
            {
                CategoryName = x.CategoryName,
                TotalQuantity = (int)x.TotalQuantity,    
                SalesPrice = (decimal)x.SalesPrice      
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ToList();
    }
}
