using FluentValidation;

namespace GROUP4PROJECT.Validations
{
    public class OrderProductValidator : AbstractValidator<Models.OrderProduct>
    {
        public OrderProductValidator(bool isPartial = false)
        {
            RuleFor(orderProduct => orderProduct.ProductId).NotEmpty();
            RuleFor(orderProduct => orderProduct.Quantity).GreaterThan(0).NotEmpty();
            RuleFor(orderProduct => orderProduct.Remarks).MinimumLength(1);
        }
    }
}