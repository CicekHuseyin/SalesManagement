using FluentValidation;
using SalesManagement.Models;

namespace SalesManagement.Validation;

public class PurchaseValidator : AbstractValidator<Purchase>
{
    public PurchaseValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("Ürün seçilmelidir.");

        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("Tedarikçi seçilmelidir.");

        RuleFor(x => x.Quantity)
            .NotNull()
            .GreaterThan(0).WithMessage("Alış miktarı 0'dan büyük olmalıdır.");

        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0).WithMessage("Alış fiyatı 0'dan büyük olmalıdır.");
    }
}
