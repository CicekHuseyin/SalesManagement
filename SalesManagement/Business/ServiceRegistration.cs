using FluentValidation;
using SalesManagement.Data.Concretes;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Concretes;
using SalesManagement.Services.Interfaces;
using SalesManagement.Validation;

namespace SalesManagement.Business;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // ============================
        // Repositories
        // ============================
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICategorySalesReportRepository, CategorySalesReportRepository>();

        // ============================
        // Services
        // ============================
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<ICategorySalesReportService, CategorySalesReportService>();
        // ============================
        // FluentValidation
        // ============================
        services.AddScoped<IValidator<Product>, ProductValidator>();
        services.AddScoped<IValidator<Category>, CategoryValidator>();
        services.AddScoped<IValidator<Customer>, CustomerValidator>();
        services.AddScoped<IValidator<Sale>, SaleValidator>();
        services.AddScoped<IValidator<Purchase>, PurchaseValidator>();

        return services;
    }
}
