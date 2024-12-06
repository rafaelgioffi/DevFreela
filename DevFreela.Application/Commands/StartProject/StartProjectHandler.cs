using DevFreela.Application.Models;
using DevFreela.Application.Notification.ProjectStarted;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.StartProject
{
    public class StartProjectHandler : IRequestHandler<StartProjectCommand, ResultViewModel>
    {
        private readonly DevFreelaDbContext _context;
        private readonly IMediator _mediator;
        public StartProjectHandler(DevFreelaDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ResultViewModel> Handle(StartProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.SingleOrDefaultAsync(p => p.Id == request.Id);

            if (project is null)
                return ResultViewModel.Error("Projeto não encontrado");

            project.Start();
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            var projectStarted = new ProjectStartedNotification(project.Id, project.Title, project.Status);
            await _mediator.Publish(projectStarted);

            return ResultViewModel.Success();
        }
    }
}
