using FluentValidation;
using SalesManagement.Core.Exceptions;
using SalesManagement.Data.Interfaces;
using SalesManagement.Models;
using SalesManagement.Services.Interfaces;

namespace SalesManagement.Services.Concretes;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IValidator<Category> _validator;

    public CategoryService(IGenericRepository<Category> categoryRepo, IValidator<Category> validator)
    {
        _categoryRepo = categoryRepo;
        _validator = validator;
    }

    public void AddCategory(Category category)
    {
        var validationResult = _validator.Validate(category);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _categoryRepo.Add(category);
        _categoryRepo.Save();
    }

    public IEnumerable<Category> GetCategories()
    {
        return _categoryRepo.GetAll();
    }

    public void UpdateCategory(Category category)
    {
        var validationResult = _validator.Validate(category);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (category.Id <= 0)
            throw new BusinessException("Geçersiz kategori bilgisi.");

        var existingCategory = _categoryRepo.Get(c => c.Id == category.Id);
        if (existingCategory == null)
            throw new BusinessException("Güncellenecek kategori bulunamadı.");

        _categoryRepo.Update(category);
        _categoryRepo.Save();
    }
}
