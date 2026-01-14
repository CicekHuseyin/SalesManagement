using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Core.Exceptions;
using SalesManagement.Data.Concretes;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Services.Concretes;

public class SaleService : ISaleService
{
    private readonly IGenericRepository<Sale> _saleRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IStockService _stockService;
    private readonly IValidator<Sale> _validator;

    public SaleService(
        IGenericRepository<Sale> saleRepo,
        IGenericRepository<Product> productRepo,
        IStockService stockService,
        IValidator<Sale> validator)
    {
        _saleRepo = saleRepo;
        _productRepo = productRepo;
        _stockService = stockService;
        _validator = validator;
    }

    /// <summary>
    /// Satış işlemini gerçekleştirir.
    /// Liste fiyatını ürün üzerinden alır, iskonto oranını hesaplar,
    /// stok kontrolü yapar ve satış sonrası stoktan düşer.
    /// </summary>
    public void AddSale(Sale sale)
    {
        // Fluent validation
        var validationResult = _validator.Validate(sale);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.Errors.First().ErrorMessage);

        // Ürün kontrolü
        var product = _productRepo.Get(p => p.Id == sale.ProductId);
        if (product == null)
            throw new BusinessException("Ürün bulunamadı.");

        // Liste fiyatı ürün tablosundan gelir
        sale.Listprice = product.Salesprice;

        if (sale.Listprice <= 0)
            throw new BusinessException("Ürünün liste fiyatı geçersiz.");

        // İskonto oranı hesaplama
        sale.Discountrate =
            ((sale.Listprice - sale.Salesprice) / sale.Listprice) * 100;

        if (sale.Discountrate < 0)
            sale.Discountrate = 0;

        // Toplam tutar
        sale.Amount = sale.Quantity * sale.Salesprice;

        sale.Date = DateTime.Now;

        // Stok kontrol + düşme
        _stockService.DecreaseStock(sale.ProductId.Value, sale.Quantity.Value);

        // Satış kaydı
        _saleRepo.Add(sale);
        _saleRepo.Save();
    }

    public Sale? GetSaleById(int id)
    {
        return _saleRepo.GetById(id);
    }

    public IEnumerable<Sale> GetSales() => _saleRepo.GetAll();

    public List<Sale> GetSalesWithProductAndCustomer()
    {
        return _saleRepo.Query()
            .Include(s => s.Product)
            .Include(s => s.Customer)
            .ToList();
    }

    public void UpdateSale(Sale sale)
    {
        // Fluent validation
        var validationResult = _validator.Validate(sale);
        if (!validationResult.IsValid)
            throw new BusinessException(validationResult.Errors.First().ErrorMessage);

        // Mevcut satış kontrolü
        var existingSale = _saleRepo.GetById(sale.Id);
        if (existingSale == null)
            throw new BusinessException("Güncellenecek satış bulunamadı.");

        // Ürün kontrolü
        var product = _productRepo.Get(p => p.Id == sale.ProductId);
        if (product == null)
            throw new BusinessException("Ürün bulunamadı.");

        // Liste fiyatı ürün tablosundan tekrar alınır
        sale.Listprice = product.Salesprice;

        if (sale.Listprice <= 0)
            throw new BusinessException("Ürünün liste fiyatı geçersiz.");

        // İskonto oranı hesaplama
        sale.Discountrate =
            ((sale.Listprice - sale.Salesprice) / sale.Listprice) * 100;

        if (sale.Discountrate < 0)
            sale.Discountrate = 0;

        // Toplam tutar
        sale.Amount = sale.Quantity * sale.Salesprice;

        // STOK FARK HESABI
        var quantityDifference =
            (sale.Quantity ?? 0) - (existingSale.Quantity ?? 0);

        if (quantityDifference > 0)
        {
            // Ek satış → stok düş
            _stockService.DecreaseStock(sale.ProductId.Value,quantityDifference);
        }
        else if (quantityDifference < 0)
        {
            // Satış azaltıldı → stok iade
            _stockService.IncreaseStock(sale.ProductId.Value,Math.Abs(quantityDifference));
        }


        // Güncellenecek alanlar
        existingSale.CustomerId = sale.CustomerId;
        existingSale.ProductId = sale.ProductId;
        existingSale.Quantity = sale.Quantity;
        existingSale.Listprice = sale.Listprice;
        existingSale.Salesprice = sale.Salesprice;
        existingSale.Discountrate = sale.Discountrate;
        existingSale.Amount = sale.Amount;

        _saleRepo.Update(existingSale);
        _saleRepo.Save();
    }

}
