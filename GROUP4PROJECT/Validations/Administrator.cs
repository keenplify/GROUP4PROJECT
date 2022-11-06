using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using GROUP4PROJECT.Models;
using GROUP4PROJECT.Core;

namespace GROUP4PROJECT.Validations
{
    public class AdministratorValidator : AbstractValidator<Models.Administrator>
    {
        public AdministratorValidator()
        { 
            RuleFor(admin => admin.Username).NotEmpty();
            RuleFor(admin => admin.Password).NotEmpty();
            RuleFor(admin => admin.Full_Name).NotEmpty();
        }
    }
}