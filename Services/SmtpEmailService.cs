using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public SmtpEmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            using var smtpClient = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort)
            {
                Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword),
                EnableSsl = _emailSettings.UseSSL
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, Attachment attachment)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            if (attachment != null)
            {
                mailMessage.Attachments.Add(attachment);
            }

            using var smtpClient = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort)
            {
                Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword),
                EnableSsl = _emailSettings.UseSSL
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

}
