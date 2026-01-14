namespace SalesManagement.ViewModels.Sales;

public class SalesViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }

    public double? Quantity { get; set; }

    public double ListPrice { get; set; }      // Ürün tablosundan çekilecek
    public double? SalesPrice { get; set; }     // Kullanıcının girdiği
    public double DiscountRate { get; set; }   // Otomatik hesaplanacak
}
