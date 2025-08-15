﻿using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Notifications
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _client;
        private readonly string _fromEmail;
        private readonly string _fromName;
        
        public EmailService(ISendGridClient client, IConfiguration configuration)
        {
            _client = client;
            _fromEmail = configuration.GetValue<string>("SendGrid:FromEmail");
            _fromName = configuration.GetValue<string>("SendGrid:FromName") ?? "";
        }

        public async Task SendAsync(string email, string subject, string message)
        {
            var sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                Subject = subject,
            };

            sendGridMessage.AddContent(MimeType.Text, message);
            sendGridMessage.AddTo(new EmailAddress(email));

            var response = _client.SendEmailAsync(sendGridMessage);
        }
    }
}
