using DevFreela.Application.Models;
using DevFreela.Application.Notification.ProjectCompleted;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.CompleteProject
{
    public class CompleteProjectHandler : IRequestHandler<CompleteProjectCommand, ResultViewModel>
    {
        private readonly DevFreelaDbContext _context;
        private readonly IMediator _mediator;
        public CompleteProjectHandler(DevFreelaDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ResultViewModel> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == request.Id);

            if (project is null)
                return ResultViewModel.Error("Projeto não encontrado");

            project.Complete();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            var projectCompleted = new ProjectCompletedNotification(project.Id, project.Title, project.Status);
            await _mediator.Publish(projectCompleted);

            return ResultViewModel.Success();
        }
    }
}
