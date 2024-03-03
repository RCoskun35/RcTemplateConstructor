using Application.StaticServices;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Users
{
    public class CreateUserValidator : AbstractValidator<User>
    {
        public CreateUserValidator()
        {
            RuleFor(p => p.Name)
               .NotEmpty().WithMessage(LanguageService.lang["alanBosBirakilamaz"]);
        }
    }
}
