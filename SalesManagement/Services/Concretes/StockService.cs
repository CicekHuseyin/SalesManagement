using SalesManagement.Core.Exceptions;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Services.Concretes;

public class StockService : IStockService
{
    private readonly IGenericRepository<Stock> _stockRepo;

    public StockService(IGenericRepository<Stock> stockRepo)
    {
        _stockRepo = stockRepo;
    }

    /// <summary>
    /// Belirtilen ürüne ait mevcut stok miktarını hesaplar.
    /// Stock tablosundaki ilgili ürün kayıtlarının Quantity alanları toplanarak bulunur.
    /// </summary>
    /// <param name="productId">Stok miktarı hesaplanacak ürünün ID bilgisi</param>
    /// <returns>Ürünün mevcut toplam stok miktarı</returns>
    public double GetCurrentStock(int productId)
    {
        return _stockRepo
            .GetAll(s => s.ProductId == productId)
            .Sum(s => s.Quantity ?? 0);
    }

    public void IncreaseStock(int productId, double quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("Stok artırma miktarı 0'dan büyük olmalıdır.");

        var stock = _stockRepo.Get(s => s.ProductId == productId);

        if (stock == null)
        {
            stock = new Stock
            {
                ProductId = productId,
                Quantity = quantity,
                Date = DateTime.Now
            };

            _stockRepo.Add(stock);
        }
        else
        {
            stock.Quantity += quantity;
            stock.Date = DateTime.Now;

            _stockRepo.Update(stock);
        }

        _stockRepo.Save();
    }


    /// <summary>
    /// Satış işlemleri sırasında ürün stoğunu azaltır.
    /// Mevcut stok miktarı kontrol edilir, yetersiz stok durumunda işlem durdurulur.
    /// Stock tablosuna negatif miktarda stok kaydı eklenir.
    /// </summary>
    /// <param name="productId">Stoğu düşürülecek ürünün ID bilgisi</param>
    /// <param name="quantity">Düşürülecek stok miktarı</param>
    public void DecreaseStock(int productId, double quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("Stok düşme miktarı 0'dan büyük olmalıdır.");

        var currentStock = GetCurrentStock(productId);

        if (currentStock < quantity)
            throw new BusinessException("Yetersiz stok.");

        var stock = new Stock
        {
            ProductId = productId,
            Quantity = -quantity, // negatif kayıt
            Date = DateTime.Now
        };

        _stockRepo.Add(stock);
        _stockRepo.Save();
    }

    /// <summary>
    /// Verilen ürün için depoda yeterli stok olup olmadığını kontrol eder.
    /// </summary>
    /// <param name="productId">Kontrol edilecek ürünün ID'si</param>
    /// <param name="quantity">Satış veya işlem için gerekli miktar</param>
    /// <returns>
    /// Eğer mevcut stok miktarı belirtilen miktara eşit veya fazlaysa true, aksi halde false döner.
    /// </returns>
    public bool HasStock(int productId, double quantity)
    {
        var stock = _stockRepo.Get(s => s.ProductId == productId);
        return (stock?.Quantity ?? 0) >= quantity;
    }
}
