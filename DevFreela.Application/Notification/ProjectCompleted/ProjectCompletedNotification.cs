using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Notification.ProjectCompleted
{
    public class ProjectCompletedNotification : INotification
    {
        public ProjectCompletedNotification(int id, string title, Enum status)
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
