using DevFreela.Application.Commands.UpdateProject;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectValidator()
        {
            RuleFor(p => p.Title)
                    .NotEmpty().WithMessage($"Título não pode ser vazio!")
                    .Length(5, 50).WithMessage("Título deve possuir entre {MinLength} e {MaxLength} caracteres. Atualmente tem {TotalLength}...");

            RuleFor(p => p.Description)
                    .NotEmpty().WithMessage($"Descrição não pode ser vazia!")
                    .Length(5, 100).WithMessage("Descrição deve ter entre {MinLength} e {MaxLength} caracteres.. Atualmente tem {TotalLength}...");

            RuleFor(p => p.IdClient)
                .GreaterThan(0).WithMessage("ID do Cliente deve ser maior que {ComparisonValue}!");

            RuleFor(p => p.IdFreelancer)
                .GreaterThan(0).WithMessage("ID do Freelancer deve ser maior que {ComparisonValue}!");

            RuleFor(p => p.TotalCost)
                .GreaterThanOrEqualTo(1000).WithMessage("O total não pode custar menos que {ComparisonValue:C}");
        }
    }
}
