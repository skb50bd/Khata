using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Business.Reports;
using Domain;
using Domain.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserRights")]
    public class ReportsController : ControllerBase
    {
        private readonly ISendEmailReport _sendEmailReport;
        private readonly IRazorViewToStringRenderer _renderer;

        public ReportsController(
            ISendEmailReport sendEmailReport, 
            IRazorViewToStringRenderer renderer)
        {
            _sendEmailReport = sendEmailReport;
            _renderer = renderer;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send()
        {
            var report = new Summary
            {
                GeneratedOn = Clock.Now,
                StartTime = Clock.Today,
                EndTime = Clock.Now
            };

            var email = new Email
            {
                Subject = $"{report.Type} Report: {report.GeneratedOn:M}",
                Message =
                    await _renderer.RenderViewToStringAsync(
                        "ReportSummary", 
                        report)
            };

            if (await _sendEmailReport.Send(email))
                return Ok();
            return BadRequest();
        }
    }
}