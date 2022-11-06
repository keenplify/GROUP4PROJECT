using FluentValidation;

namespace GROUP4PROJECT.Validations
{
    public class ProductValidator : AbstractValidator<Models.Product>
    {
        public ProductValidator(bool isPartial=false)
        {
            RuleFor(product => product.Price).NotNull().GreaterThan(0);
            RuleFor(product => product.Name).NotNull().MinimumLength(1);
            RuleFor(product => product.Description).NotNull().MinimumLength(1);
        }
    }
}