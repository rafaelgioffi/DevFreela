using DevFreela.Application.Models;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class CreateSkillValidator : AbstractValidator<CreateSkillInputModel>
    {
        public CreateSkillValidator()
        {
            RuleFor(s => s.Description)
                .NotEmpty().WithMessage("A descrição não pode ser vazia")
                .Length(1, 30).WithMessage("A Skill deve ter entre {MinLength} e {MaxLength} caracteres.. Atualmente tem {TotalLength}...");
        }
    }


}
