using BusinessLogicLayer.DTOs;
using FluentValidation;

namespace BusinessLogicLayer.Validators
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).When(x => x.UnitPrice.HasValue)
                .WithMessage("Unit price must be non-negative.");
            RuleFor(x => x.QuantityInStock)
                .GreaterThanOrEqualTo(0).When(x => x.QuantityInStock.HasValue)
                .WithMessage("Quantity in stock must be non-negative.");
        }
    }
}