using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using GROUP4PROJECT.Models;
using GROUP4PROJECT.Core;

namespace GROUP4PROJECT.Validations
{
    public class CashierValidator : AbstractValidator<Cashier>
    {
        public CashierValidator(bool isPartial = false)
        { 
            if (isPartial)
            {
                RuleFor(cashier => cashier.Username);
                RuleFor(cashier => cashier.Password);
                RuleFor(cashier => cashier.FullName);
            } else
            {
                RuleFor(cashier => cashier.Username).NotEmpty();
                RuleFor(cashier => cashier.Password).NotEmpty();
                RuleFor(cashier => cashier.FullName).NotEmpty();
            }
        }
    }
}