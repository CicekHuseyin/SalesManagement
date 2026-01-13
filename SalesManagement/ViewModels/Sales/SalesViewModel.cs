namespace SalesManagement.ViewModels.Sales;

public class SalesViewModel
{
    public int ProductId { get; set; }
    public int CustomerId { get; set; }

    // Entity’de Quantity double? olduğu için ViewModel’de de double kullanalım
    public double Quantity { get; set; }

    public double ListPrice { get; set; }      // Ürün tablosundan çekilecek
    public double? SalesPrice { get; set; }     // Kullanıcının girdiği
    public double DiscountRate { get; set; }   // Otomatik hesaplanacak
}
