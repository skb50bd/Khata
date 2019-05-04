using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Reports
{
    public class SendEmailReport : ISendEmailReport
    {
        private readonly OutletOptions _settings;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SendEmailReport> _logger;

        public IEnumerable<string> RecepientAddresses =>
            _settings.Email.Split(',')
                     .Select(e => e.Trim());

        public SendEmailReport(
            IOptionsMonitor<OutletOptions> optionsMonitor, 
            IEmailSender emailSender, 
            ILogger<SendEmailReport> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
            _settings = optionsMonitor.CurrentValue;
        }

        public async Task<bool> Send(Email email)
        {
            try
            {
                foreach (var address in RecepientAddresses)
                {
                    _logger.LogInformation($"Sending Email Report to {address}");
                    await _emailSender.SendEmailAsync(
                        address,
                        email.Subject,
                        email.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Error Sending Email Record: {e.Message}", 
                    e);
                return false;
            }

            return true;
        }
    }
}
