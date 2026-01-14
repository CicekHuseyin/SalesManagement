namespace SalesManagement.ViewModels;

public class PurchaseViewModel
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public int CustomerId { get; set; }

    public double Quantity { get; set; }
    public double Price { get; set; }

    public double Amount { get; set; }
}
