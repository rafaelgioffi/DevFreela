using MediatR;

namespace DevFreela.Application.Notification.ProjectStarted
{
    public class FreelanceNotificationHandler : INotificationHandler<ProjectStartedNotification>
    {
        public Task Handle(ProjectStartedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Olá freelancer! O projeto denominado '{notification.Title}' teve seu status alterado para '{notification.Status}'.");

            return Task.CompletedTask;
        }

    }
}
