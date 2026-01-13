using FluentValidation;
using SalesManagement.Models;

namespace SalesManagement.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ürün adı boş olamaz.")
            .MaximumLength(100);

        RuleFor(x => x.Salesprice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Satış fiyatı 0'dan küçük olamaz.");
    }
}
