using Microsoft.AspNetCore.Mvc;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;
using SalesManagement.ViewModels.Products;

namespace SalesManagement.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetCategories();
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View(model);
        }

        _productService.AddProduct(new Product
        {
            Name = model.Name,
            Salesprice = model.Salesprice,
            CategoryId = model.CategoryId
        });

        TempData["ProductSuccess"] = "Ürün başarıyla kaydedildi";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var product = _productService.GetProducts()
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
            return NotFound();

        ViewBag.Categories = _categoryService.GetCategories();

        return View(new ProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Salesprice = product.Salesprice,
            CategoryId = product.CategoryId
        });
    }

    [HttpPost]
    public IActionResult Edit(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View(model);
        }

        // Önce mevcut product'ı çek
        var existingProduct = _productService.GetProductById(model.Id);

        if (existingProduct != null)
        {
            existingProduct.Name = model.Name;
            existingProduct.Salesprice = model.Salesprice;
            existingProduct.CategoryId = model.CategoryId;

            _productService.UpdateProduct(existingProduct);
        }
        TempData["ProductSuccess"] = "Ürün başarıyla güncellendi";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult GetProducts()
    {
        var products = _productService.GetProductsWithCategory()
        .Select(p => new ProductListViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Salesprice = p.Salesprice,
            CategoryName = p.Category != null ? p.Category.Name : ""
        }).ToList();

        return Json(products);
    }
}
