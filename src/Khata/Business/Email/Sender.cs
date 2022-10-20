using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Fmail = FluentEmail.Core.Email;


namespace Business.Email;

public class Sender : IEmailSender
{
    private readonly Settings _settings;
    public Sender(
        IOptionsMonitor<Settings> opts) => 
        _settings = opts.CurrentValue;

    public async Task SendEmailAsync(
        string recipientAddress,
        string subject,
        string message)
    {
        Fmail.DefaultSender = new MailgunSender(
            _settings.MailGunDomain,
            _settings.MailGunApiKey
        );

        var email =
            Fmail.From(
                    _settings.SenderAddress, 
                    _settings.SenderName)
                .To(recipientAddress)
                .Subject(subject)
                .Body(message, true);

        var response = 
            await email.SendAsync();

        if (!response.Successful)
            throw new Exception(
                @"Sending Email Failed.
                         Error Messages:" +
                string.Join(",\n", 
                    response.ErrorMessages));
    }
}