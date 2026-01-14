using Microsoft.AspNetCore.Mvc;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;
using SalesManagement.ViewModels.Purchase;

namespace SalesManagement.Controllers;

public class PurchaseController : Controller
{
    private readonly IPurchaseService _purchaseService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly IStockService _stockService;

    public PurchaseController(
        IPurchaseService purchaseService,
        IProductService productService,
        ICustomerService customerService,
        IStockService stockService)
    {
        _purchaseService = purchaseService;
        _productService = productService;
        _customerService = customerService;
        _stockService = stockService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.Products = _productService.GetProducts() ?? new List<Product>();
        ViewBag.Customers = _customerService.GetCustomers() ?? new List<Customer>();
        return View();
    }

    [HttpPost]
    public IActionResult Create(PurchaseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Products = _productService.GetProducts();
            ViewBag.Customers = _customerService.GetCustomers();
            return View(model);
        }

        var purchase = new Purchase
        {
            ProductId = model.ProductId,
            CustomerId = model.CustomerId,
            Quantity = model.Quantity,
            Price = model.Price,
            Amount = (model.Price) * (model.Quantity),
            Date = DateTime.Now
        };

        _purchaseService.AddPurchase(purchase);

        TempData["PurchaseSuccess"] = "Alış kaydı başarıyla eklendi";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var purchase = _purchaseService.GetPurchaseById(id);
        if (purchase == null)
            return NotFound();

        var model = new PurchaseViewModel
        {
            Id = purchase.Id,
            ProductId = purchase.ProductId ?? 0,
            CustomerId = purchase.CustomerId ?? 0,
            Quantity = purchase.Quantity ?? 0,
            Price = purchase.Price ?? 0
        };

        ViewBag.Products = _productService.GetProducts();
        ViewBag.Customers = _customerService.GetCustomers();

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(PurchaseViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Products = _productService.GetProducts();
            ViewBag.Customers = _customerService.GetCustomers();
            return View(model);
        }

        var purchase = _purchaseService.GetPurchaseById(model.Id);
        if (purchase == null)
            return NotFound();

        // Stok fark hesabı
        var quantityDifference = (model.Quantity) - (purchase.Quantity ?? 0);

        if (quantityDifference > 0)
        {
            _stockService.IncreaseStock(model.ProductId, quantityDifference);
        }
        else if (quantityDifference < 0)
        {
            _stockService.DecreaseStock(model.ProductId, Math.Abs(quantityDifference));
        }

        // Güncelle
        purchase.ProductId = model.ProductId;
        purchase.CustomerId = model.CustomerId;
        purchase.Quantity = model.Quantity;
        purchase.Price = model.Price;
        purchase.Amount = (model.Price) * (model.Quantity);

        _purchaseService.UpdatePurchase(purchase);

        TempData["PurchaseSuccess"] = "Alış kaydı başarıyla güncellendi";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult GetPurchases()
    {
        var purchases = _purchaseService.GetPurchases()
            .Select(p => new
            {
                p.Id,
                ProductName = p.Product?.Name,
                CustomerName = p.Customer != null
                    ? $"{p.Customer.Customertitle} ({p.Customer.Customernumber})"
                    : "",
                p.Quantity,
                p.Price,
                p.Amount,
                p.Date
            }).ToList();

        return Json(purchases);
    }
}
