using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Business.Auth
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpEmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;

        public SmtpEmailSender(
            IOptions<SmtpEmailSettings> emailSettings,
            IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(
            string receipient, 
            string subject, 
            string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(
                    new MailboxAddress(
                        _emailSettings.SenderName,
                        _emailSettings.Sender)
                );

                message.To.Add(
                    new MailboxAddress(
                        "KHATA Admin",
                        receipient)
                );

                message.Subject = subject;

                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback =
                        (s, c, h, e) => true;

                    await client.ConnectAsync(
                        _emailSettings.MailServer, 
                        _emailSettings.MailPort, 
                        false);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(
                        _emailSettings.Sender, 
                        _emailSettings.Password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

    }
}