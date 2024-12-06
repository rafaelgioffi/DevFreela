using DevFreela.Application.Commands.InsertComment;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class InsertCommentValidator : AbstractValidator<InsertCommentCommand>
    {
        public InsertCommentValidator()
        {
            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Comentário não pode ser vazio!")
                .Length(10, 100).WithMessage("Comentário deve ter entre {MinLength} e {MaxLength} caracteres.. Atualmente tem {TotalLength}...");

            RuleFor(p => p.IdProject)
                .GreaterThan(0).WithMessage("ID do Projeto deve ser maior que {ComparisonValue}!");

            RuleFor(p => p.IdUser)
                .GreaterThan(0).WithMessage("ID do Usuário deve ser maior que {ComparisonValue}!");
        }
    }
}