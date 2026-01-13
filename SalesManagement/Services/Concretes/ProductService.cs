using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Core.Exceptions;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Services.Concretes;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IValidator<Product> _validator;

    public ProductService(IGenericRepository<Product> productRepo, IValidator<Product> validator)
    {
        _productRepo = productRepo;
        _validator = validator;
    }

    public void AddProduct(Product product)
    {
        var validationResult = _validator.Validate(product);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _productRepo.Add(product);
        _productRepo.Save();
    }

    public IEnumerable<Product> GetProducts()
    {
        return _productRepo.GetAll();
    }

    public IEnumerable<Product> GetProductsWithCategory()
    {
        return _productRepo
        .Query()
        .Include(p => p.Category)
        .ToList();
    }

    public void UpdateProduct(Product product)
    {
        var validationResult = _validator.Validate(product);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (product.Id <= 0)
            throw new BusinessException("Geçersiz ürün bilgisi.");

        // Repo'dan mevcut entity'yi çek
        var existingProduct = _productRepo.Get(p => p.Id == product.Id);
        if (existingProduct == null)
            throw new BusinessException("Güncellenecek ürün bulunamadı.");

        // Mevcut entity üzerinde güncelleme yap
        existingProduct.Name = product.Name;
        existingProduct.Salesprice = product.Salesprice;
        existingProduct.CategoryId = product.CategoryId;

        _productRepo.Update(existingProduct);
        _productRepo.Save();
    }

    public Product GetProductById(int id)
    {
        return _productRepo.GetById(id);
    }
}
