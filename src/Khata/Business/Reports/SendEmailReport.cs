using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Reports;
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
        private readonly IReportService<Summary> _summaryService;

        public IEnumerable<string> RecepientAddresses =>
            _settings.Email.Split(',')
                     .Select(e => e.Trim());

        public SendEmailReport(
            IOptionsMonitor<OutletOptions> optionsMonitor,
            IEmailSender emailSender,
            ILogger<SendEmailReport> logger,
            IReportService<Summary> summaryService)
        {
            _emailSender = emailSender;
            _logger = logger;
            _summaryService = summaryService;
            _settings = optionsMonitor.CurrentValue;
        }

        public async Task<bool> Send(string subject, string body)
        {
            try
            {
                foreach (var address in RecepientAddresses)
                {
                    _logger.LogInformation($"Sending Email Report to {address}");
                    await _emailSender.SendEmailAsync(address, subject, body);
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

        public async Task<Summary> GetReport()
        {
            return await _summaryService.Get();
        }
    }
}
