using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using Sales.Options;

namespace Sales.Services
{
    /// <summary>
    /// Сервис отправки сообщений
    /// </summary>
    public class EmailSenderService : IEmailSender
    {
        private readonly MailOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public EmailSenderService(IOptions<MailOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Отправка сообщения по email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // создание сообщения
            var fromEmail = new MimeMessage();
            fromEmail.From.Add(MailboxAddress.Parse(_options.Email));
            fromEmail.To.Add(MailboxAddress.Parse(email));
            fromEmail.Subject = subject;
            fromEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            // отправка email
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_options.SmtpHost, _options.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await smtp.AuthenticateAsync(_options.SmtpUser, _options.SmtpPassword);
            await smtp.SendAsync(fromEmail);
            await smtp.DisconnectAsync(true);
        }
    }
}
