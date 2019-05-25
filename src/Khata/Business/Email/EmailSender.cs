using System;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;


namespace Business
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        public EmailSender(IOptionsMonitor<EmailSettings> opts)
        {
            _settings = opts.CurrentValue;
        }


        public async Task SendEmailAsync(
            string recipientAddress,
            string subject,
            string message)
        {
            Email.DefaultSender = new MailgunSender(
                _settings.MailGunDomain,
                _settings.MailGunApiKey
            );

            var mail =
                Email.From(
                         _settings.SenderAddress,
                         _settings.SenderName)
                    .To(recipientAddress)
                    .Subject(subject)
                    .Body(message, true);

            var response = await mail.SendAsync();
            if(!response.Successful)
                throw new Exception("Sending Email Failed." +
                                    "\nError Messages:" +
                                    "\n" + 
                                    string.Join(
                                        ",\n", 
                                        response.ErrorMessages) );
            //return response.Successful 
            //    ? "Success" 
            //    : string.Join(
            //        ",\n", 
            //        response.ErrorMessages);
        }
    }
}
