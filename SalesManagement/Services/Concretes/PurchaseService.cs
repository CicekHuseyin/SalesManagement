using FluentValidation;
using SalesManagement.Core.Exceptions;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Services.Concretes;

public class PurchaseService : IPurchaseService
{
    private readonly IGenericRepository<Purchase> _purchaseRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IStockService _stockService;
    private readonly IValidator<Purchase> _validator;

    public PurchaseService(
        IGenericRepository<Purchase> purchaseRepo,
        IGenericRepository<Product> productRepo,
        IStockService stockService,
        IValidator<Purchase> validator)
    {
        _purchaseRepo = purchaseRepo;
        _productRepo = productRepo;
        _stockService = stockService;
        _validator = validator;
    }

    /// <summary>
    /// Alış işlemini gerçekleştirir.
    /// Toplam tutarı hesaplar ve alış sonrası stoğu artırır.
    /// </summary>
    public void AddPurchase(Purchase purchase)
    {
        // Fluent validation
        var validationResult = _validator.Validate(purchase);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.Errors.First().ErrorMessage);

        // Ürün kontrolü
        var product = _productRepo.Get(p => p.Id == purchase.ProductId);
        if (product == null)
            throw new BusinessException("Ürün bulunamadı.");

        // Toplam tutar
        purchase.Amount = purchase.Quantity * purchase.Price;

        purchase.Date = DateTime.Now;

        // Stok artırma
        _stockService.IncreaseStock(purchase.ProductId.Value,purchase.Quantity.Value);

        // Alış kaydı
        _purchaseRepo.Add(purchase);
        _purchaseRepo.Save();
    }

    public IEnumerable<Purchase> GetPurchases()
    {
        return _purchaseRepo.GetAll();
    }
}
