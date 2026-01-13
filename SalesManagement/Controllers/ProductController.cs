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

    // LIST
    public IActionResult Index()
    {
        var products = _productService.GetProductsWithCategory()
        .Select(p => new ProductListViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Salesprice = p.Salesprice,
            CategoryName = p.Category != null ? p.Category.Name : ""
        }).ToList();

        return View(products);
    }

    // CREATE GET
    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetCategories();
        return View();
    }

    // CREATE POST
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

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
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

    // EDIT POST
    [HttpPost]
    public IActionResult Edit(ProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View(model);
        }

        _productService.UpdateProduct(new Product
        {
            Id = model.Id,
            Name = model.Name,
            Salesprice = model.Salesprice,
            CategoryId = model.CategoryId
        });

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

        return Ok(products);
    }
}
