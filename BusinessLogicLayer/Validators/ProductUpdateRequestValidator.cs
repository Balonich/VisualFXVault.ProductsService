using BusinessLogicLayer.DTOs;
using FluentValidation;

namespace BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequestDto>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("Category cannot exceed 50 characters.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).When(x => x.UnitPrice.HasValue).WithMessage("Unit price must be non-negative.");

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0).When(x => x.QuantityInStock.HasValue).WithMessage("Quantity in stock must be non-negative.");
    }
}