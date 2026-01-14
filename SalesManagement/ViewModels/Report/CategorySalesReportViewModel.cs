namespace SalesManagement.ViewModels.Report;

public class CategorySalesReportViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public decimal SalesPrice { get; set; }
}
