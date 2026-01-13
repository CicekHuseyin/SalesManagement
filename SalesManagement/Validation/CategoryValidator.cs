using FluentValidation;
using SalesManagement.Models;

namespace SalesManagement.Validation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Kategori bilgisi boş olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Kategori adı boş olamaz.")
                .MaximumLength(100);
        }
    }
}
