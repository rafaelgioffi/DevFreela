using DevFreela.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Validators
{
    public class CreateUserValidator  :AbstractValidator<CreateUserInputModel>
    {
        public CreateUserValidator() 
        {
            RuleFor(u => u.Email)
                    .EmailAddress().WithMessage("E-mail deve possuir um formato válido. endereço@provedor.domínio. Ex.: pessoa@email.com.br");

            RuleFor(u => u.BirthDate)
                .Must(d => d < DateTime.Now.AddYears(-18)).WithMessage($"Deve ser maior de idade.");
        }
    }
}
