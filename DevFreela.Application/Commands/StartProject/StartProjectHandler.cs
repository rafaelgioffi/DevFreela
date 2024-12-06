using DevFreela.Application.Models;
using DevFreela.Application.Notification.ProjectStarted;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.StartProject
{
    public class StartProjectHandler : IRequestHandler<StartProjectCommand, ResultViewModel>
    {
        private readonly IMediator _mediator;
        private readonly IProjectRepository _repository;
        public StartProjectHandler(IMediator mediator, IProjectRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ResultViewModel> Handle(StartProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetById(request.Id);

            if (project is null)
                return ResultViewModel.Error("Projeto não encontrado");

            project.Start();
            await _repository.Update(project);

            var projectStarted = new ProjectStartedNotification(project.Id, project.Title, project.Status);
            await _mediator.Publish(projectStarted);

            return ResultViewModel.Success();
        }
    }
}
