using MediatR;

namespace DevFreela.Application.Notification.ProjectCompleted
{
    public class FreelanceNotificationHandler : INotificationHandler<ProjectCompletedNotification>
    {
        public Task Handle(ProjectCompletedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Olá freelancer! O projeto denominado '{notification.Title}' teve seu status alterado para '{notification.Status}'.");

            return Task.CompletedTask;
        }
    }
}
