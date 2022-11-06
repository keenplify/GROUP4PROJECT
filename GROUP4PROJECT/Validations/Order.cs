using FluentValidation;

namespace GROUP4PROJECT.Validations
{
    public class OrderValidator : AbstractValidator<Models.Order>
    {
        public OrderValidator(bool isPartial = false)
        {
            RuleFor(order => order.CustomerName).NotNull().MinimumLength(1).MaximumLength(20);
            RuleForEach(order => order.OrderProducts).SetValidator(new OrderProductValidator()).NotNull();
        }
    }
}