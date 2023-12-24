using System.Net.Mail;

namespace OnlineStore.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);

        Task SendEmailWithAttachmentAsync(string to, string subject, string body, Attachment attachment);

    }
}
