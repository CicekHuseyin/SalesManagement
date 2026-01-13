namespace SalesManagement.ViewModels.Products;

public class ProductViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double? Salesprice { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }
}
