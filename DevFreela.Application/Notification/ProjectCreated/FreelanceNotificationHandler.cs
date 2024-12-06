using MediatR;

namespace DevFreela.Application.Notification.ProjectCreated
{
    public class FreelanceNotificationHandler : INotificationHandler<ProjectCreatedNotification>
    {
        public Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Olá freelancer! Um novo projeto denominado '{notification.Title}' foi criado! Venha conferir!");

            return Task.CompletedTask;
        }
    }

}
