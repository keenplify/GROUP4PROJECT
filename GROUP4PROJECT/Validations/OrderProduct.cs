using FluentValidation;

namespace GROUP4PROJECT.Validations
{
    public class OrderProductValidator : AbstractValidator<Models.OrderProduct>
    {
        public OrderProductValidator(bool isPartial = false)
        {
            RuleFor(orderProduct => orderProduct.ProductId).NotNull();
            RuleFor(orderProduct => orderProduct.Quantity).GreaterThan(0).NotNull();
            RuleFor(orderProduct => orderProduct.Remarks).MinimumLength(1);
        }
    }
}