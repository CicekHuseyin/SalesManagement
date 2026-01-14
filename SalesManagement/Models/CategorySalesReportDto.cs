namespace SalesManagement.Models;

public class CategorySalesReportDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Quantity { get; set; }
    public double SalesPrice { get; set; }
    public double DiscountRate { get; set; }
    public DateTime? Date { get; set; }
    public double TotalQuantity { get; set; }
}
