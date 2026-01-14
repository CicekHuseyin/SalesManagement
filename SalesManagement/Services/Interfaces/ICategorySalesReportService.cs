using SalesManagement.ViewModels.Report;

namespace SalesManagement.Services.Interfaces;

public interface ICategorySalesReportService
{
    IEnumerable<CategorySalesReportViewModel> GetCategorySalesReport();
}
