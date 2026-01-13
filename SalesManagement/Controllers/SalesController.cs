using Microsoft.AspNetCore.Mvc;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;
using SalesManagement.ViewModels.Sales;

namespace SalesManagement.Controllers;

public class SalesController : Controller
{
    private readonly IProductService _productService;
    private readonly ISaleService _salesService;
    private readonly IStockService _stockService;
    private readonly ICustomerService _customerService;

    public SalesController(IProductService productService, ISaleService salesService,IStockService stockService, ICustomerService customerService)
    {
        _productService = productService;
        _salesService = salesService;
        _stockService = stockService;
        _customerService = customerService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        // Ürünleri ve müşterileri dropdown olarak gönder
        ViewBag.Products = _productService.GetProducts() ?? new List<Product>();
        ViewBag.Customers = _customerService.GetCustomers() ?? new List<Customer>();

        return View(); // Modeli başlatıyoruz
    }

    [HttpPost]
    public IActionResult Create(SalesViewModel model)
    {
        // Model boş mu kontrol et
        if (model == null)
        {
            ModelState.AddModelError("", "Geçersiz veri gönderildi.");
            ViewBag.Products = _productService.GetProducts() ?? new List<Product>();
            ViewBag.Customers = _customerService.GetCustomers() ?? new List<Customer>();
            return View(new SalesViewModel());
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Products = _productService.GetProducts() ?? new List<Product>();
            ViewBag.Customers = _customerService.GetCustomers() ?? new List<Customer>();
            return View(model);
        }

        var product = _productService.GetProductById(model.ProductId);
        if (product == null)
        {
            ModelState.AddModelError("", "Ürün bulunamadı!");
            ViewBag.Products = _productService.GetProducts() ?? new List<Product>();
            ViewBag.Customers = _customerService.GetCustomers() ?? new List<Customer>();
            return View(model);
        }

        // SalesPrice null olursa 0 olarak al
        model.ListPrice = product.Salesprice ?? 0;
        model.SalesPrice = model.SalesPrice; // Eğer kullanıcı girdiği değer null ise 0
        model.DiscountRate = model.ListPrice != 0
            ? ((model.ListPrice - (model.SalesPrice ?? 0)) / model.ListPrice * 100)
            : 0;

        var sale = new Sale
        {
            ProductId = model.ProductId,
            CustomerId = model.CustomerId,
            Quantity = model.Quantity,
            Listprice = model.ListPrice,
            Salesprice = model.SalesPrice ?? 0.0,
            Discountrate = model.DiscountRate,
            Date = DateTime.Now,
            Amount = (model.SalesPrice ?? 0.0) * model.Quantity
        };

        _salesService.AddSale(sale);
        _stockService.DecreaseStock(model.ProductId, model.Quantity);

        TempData["SaleSuccess"] = "Satış başarıyla kaydedildi";

        return RedirectToAction(nameof(Index));
    }

    //public IActionResult Edit(int id)
    //{
    //    var sale = _salesService.GetSaleById(id);
    //    if (sale == null)
    //        return NotFound();

    //    var model = new SalesViewModel
    //    {
    //        Id = sale.Id,
    //        ProductId = sale.ProductId,
    //        CustomerId = sale.CustomerId,
    //        Quantity = sale.Quantity,
    //        ListPrice = sale.Listprice,
    //        SalesPrice = sale.Salesprice,
    //        DiscountRate = sale.Discountrate
    //    };

    //    ViewBag.Products = _productService.GetProducts() ?? new List<Product>();
    //    ViewBag.Customers = _customerService.GetCustomers() ?? new List<Customer>();

    //    return View(model);
    //}

    //[HttpPost]
    //public IActionResult Edit(SalesViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        ViewBag.Products = _productService.GetProducts();
    //        ViewBag.Customers = _customerService.GetCustomers();
    //        return View(model);
    //    }

    //    var sale = _salesService.GetSaleById(model.ProductId);
    //    if (sale == null)
    //        return NotFound();

    //    var product = _productService.GetProductById(model.ProductId);
    //    if (product == null)
    //    {
    //        ModelState.AddModelError("", "Ürün bulunamadı");
    //        return View(model);
    //    }

    //    // Liste fiyatını üründen tekrar al
    //    model.ListPrice = product.Salesprice ?? 0;

    //    model.DiscountRate = model.ListPrice > 0
    //        ? ((model.ListPrice - (model.SalesPrice ?? 0)) / model.ListPrice * 100)
    //        : 0;

    //    // STOK FARK HESABI
    //    var quantityDifference = model.Quantity - sale.Quantity;

    //    if (quantityDifference > 0)
    //    {
    //        // Ek satış → stok düş
    //        _stockService.DecreaseStock(model.ProductId, quantityDifference);
    //    }
    //    else if (quantityDifference < 0)
    //    {
    //        // Satış azaltıldı → stok iade
    //        _stockService.IncreaseStock(model.ProductId, Math.Abs(quantityDifference));
    //    }

    //    // Satışı güncelle
    //    sale.CustomerId = model.CustomerId;
    //    sale.Quantity = model.Quantity;
    //    sale.Listprice = model.ListPrice;
    //    sale.Salesprice = model.SalesPrice ?? 0;
    //    sale.Discountrate = model.DiscountRate;
    //    sale.Amount = sale.Salesprice * sale.Quantity;

    //    _salesService.UpdateSale(sale);

    //    TempData["SaleSuccess"] = "Satış başarıyla güncellendi";

    //    return RedirectToAction(nameof(Index));
    //}

    // Ürün listesini getirme
    public IActionResult GetSales()
    {
        var sales = _salesService.GetSalesWithProductAndCustomer()
            .Select(s => new
            {
                s.Id,
                ProductName = s.Product?.Name,
                CustomerName = s.Customer != null ? $"{s.Customer.Customertitle} ({s.Customer.Customernumber})" : "",
                s.Quantity,
                s.Listprice,
                s.Salesprice,
                s.Discountrate,
                s.Date
            }).ToList();

        return Json(sales);
    }

    [HttpGet]
    public IActionResult GetProductPrice(int id)
    {
        var product = _productService.GetProductById(id);

        if (product == null)
            return Json(null);

        return Json(new
        {
            listPrice = product.Salesprice   
        });
    }

}
