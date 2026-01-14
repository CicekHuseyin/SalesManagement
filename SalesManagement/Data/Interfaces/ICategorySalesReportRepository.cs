using SalesManagement.Models;

namespace SalesManagement.Data.Interfaces;

public interface ICategorySalesReportRepository
{
    IEnumerable<CategorySalesReportDto> GetAllSalesDetail();
}
