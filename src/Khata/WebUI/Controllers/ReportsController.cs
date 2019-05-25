using Business.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var report =
                await _sendEmailReport.GetReport();

            var subject = $"{report.Type} Report: {report.GeneratedOn:M}";
            var body = await _renderer.RenderViewToStringAsync("ReportSummary", report);

            if (await _sendEmailReport.Send(subject, body))
                return Ok();

            return BadRequest();
        }
    }
}