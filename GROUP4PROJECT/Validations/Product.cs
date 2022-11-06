using FluentValidation;

namespace GROUP4PROJECT.Validations
{
    public class ProductValidator : AbstractValidator<Models.Product>
    {
        public ProductValidator(bool isPartial=false)
        {
            RuleFor(product => product.Price).NotEmpty().GreaterThan(0);
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.Description).NotEmpty();
            RuleFor(product => product.CategoryId).NotEmpty();
            RuleFor(product => product.ImageUrl).NotEmpty();
        }
    }
}