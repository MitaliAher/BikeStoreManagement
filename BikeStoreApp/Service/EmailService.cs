using BikeStoreApp.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BikeStoreApp.Service
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings?.Value ?? throw new ArgumentNullException(nameof(smtpSettings));
            
            // Validate SMTP settings
            if (string.IsNullOrEmpty(_smtpSettings.SmtpServer))
                throw new ArgumentNullException(nameof(_smtpSettings.SmtpServer));

            if (_smtpSettings.Port <= 0)
                throw new ArgumentException("Port must be a positive integer", nameof(_smtpSettings.Port));

            if (string.IsNullOrEmpty(_smtpSettings.UserName))
                throw new ArgumentNullException(nameof(_smtpSettings.UserName));

            if (string.IsNullOrEmpty(_smtpSettings.Password))
                throw new ArgumentNullException(nameof(_smtpSettings.Password));

            if (string.IsNullOrEmpty(_smtpSettings.SenderEmail))
                throw new ArgumentNullException(nameof(_smtpSettings.SenderEmail));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.SmtpServer,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
