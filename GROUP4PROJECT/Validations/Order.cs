using FluentValidation;
using GROUP4PROJECT.Enums;

namespace GROUP4PROJECT.Validations
{
    public class OrderValidator : AbstractValidator<Models.Order>
    {
        public OrderValidator(bool isStatusOnly = false)
        {
            if (isStatusOnly)
            {
                RuleFor(order => order.Status).IsEnumName(typeof(OrderStatus));
            } else
            {
                RuleFor(order => order.Status).IsEnumName(typeof(OrderStatus));
                RuleFor(order => order.Type).IsEnumName(typeof(OrderType));
                RuleForEach(order => order.OrderProducts).SetValidator(new OrderProductValidator()).NotEmpty();
            }
        }
    }
}