using FluentValidation;
using SalesManagement.Models;

namespace SalesManagement.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("Ürün seçilmelidir.");

        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("Müşteri seçilmelidir.");

        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThan(0).WithMessage("Satış miktarı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Salesprice)
            .NotNull()
            .GreaterThan(0).WithMessage("Satış fiyatı 0'dan büyük olmalıdır.");
    }
}
