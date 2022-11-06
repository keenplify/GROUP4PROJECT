using FluentValidation;

namespace GROUP4PROJECT.Validations
{
    public class CategoryValidator : AbstractValidator<Models.Category>
    {
        public CategoryValidator(bool isPartial=false)
        {
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.ImageUrl).NotEmpty();
        }
    }
}