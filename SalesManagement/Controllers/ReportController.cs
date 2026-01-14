using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Controllers;

public class ReportController : Controller
{
    private readonly TestDbContext _context;
    private readonly IStockService _stockService;

    public ReportController(
        TestDbContext context,
        IStockService stockService)
    {
        _context = context;
        _stockService = stockService;
    }

    public IActionResult CategorySalesReport()
    {
        return View();
    }

    /// <summary>
    /// Stored Procedure kullanılarak kategori bazlı satış raporu getirildi.
    /// En çok satılandan en aza doğru sıralı şekilde listelenmektedir.
    /// </summary>
    [HttpGet]
    public IActionResult GetCategorySalesReport()
    {
        var result = _context.CategorySalesReportDtos
            .FromSqlRaw("EXEC SPReportGetAllSalesDetail")
            .AsEnumerable()
            .Select(x => new
            {
                categoryName = x.CategoryName,
                totalQuantity = x.TotalQuantity,
                salesPrice = x.SalesPrice
            })
            .OrderByDescending(x => x.totalQuantity)
            .ToList();

        return Json(result);
    }

    public IActionResult StockReport()
    {
        return View();
    }

    /// <summary>
    /// Ürün bazlı toplam stok durumu raporu.
    /// Tarih aralığı gerekmez.
    /// </summary>
    [HttpGet]
    public IActionResult GetStockReport()
    {
        var stocks = _stockService.GetStocks()
            .GroupBy(x => new
            {
                x.ProductId,
                ProductName = x.Product != null ? x.Product.Name : "Tanımsız Ürün"
            })
            .Select(g => new
            {
                productName = g.Key.ProductName, // JS tarafı için camelCase
                totalStock = g.Sum(x => x.Quantity)
            })
            .ToList();

        return Json(stocks);
    }


}
