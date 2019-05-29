using System.Threading.Tasks;

using Brotal.Extensions;

using Business;

using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Policy = "AdminRights")]
    [Route("api/[controller]")]
    public class BackupRestoreController : Controller
    {
        private readonly BackupRestoreService _brs;

        public BackupRestoreController(
            BackupRestoreService brs) =>
            _brs = brs;

        // GET: api/BackupRestore
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stream = await _brs.GetJsonDump();
            var file = File(
                stream,
                "application/octet-stream",
                $"backup-{Clock.Now.Timestamp()}.zip");
            //stream.Close();
            //stream.Dispose();
            return file;
        }
        //// POST api/BackupRestore
        //[HttpPost]
        //public void Post([FromBody]IFormFile backupFile)
        //{

        //}
    }
}
