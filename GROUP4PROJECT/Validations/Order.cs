using FluentValidation;
using GROUP4PROJECT.Enums;

namespace GROUP4PROJECT.Validations
{
    public class OrderValidator : AbstractValidator<Models.Order>
    {
        public OrderValidator(bool isPartial = false)
        {
            RuleFor(order => order.CustomerName).NotEmpty().MinimumLength(1).MaximumLength(20);
            RuleFor(order => order.Status).IsEnumName(typeof(OrderStatus));
            RuleFor(order => order.Type).IsEnumName(typeof(OrderType));
            RuleForEach(order => order.OrderProducts).SetValidator(new OrderProductValidator()).NotEmpty();
        }
    }
}