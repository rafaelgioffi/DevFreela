using MediatR;

namespace DevFreela.Application.Notification.ProjectStarted
{
    public class ProjectStartedNotification : INotification
    {
        public ProjectStartedNotification(int id, string title, Enum status)
        {
            Id = id;
            Title = title;
            Status = status;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public Enum Status { get; private set; }
    }
}
