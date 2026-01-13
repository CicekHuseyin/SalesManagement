using FluentValidation;
using SalesManagement.Models;

namespace SalesManagement.Validation
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Müşteri bilgisi boş olamaz.");

            RuleFor(x => x.Customertitle)
                .NotEmpty()
                .WithMessage("Müşteri adı / unvanı boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.Customernumber)
                .NotEmpty()
                .WithMessage("Müşteri numarası boş olamaz.")
                .MaximumLength(50);
        }
    }
}
