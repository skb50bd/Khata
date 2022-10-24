using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

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
    public async Task<IActionResult> Get() =>
        File(await _brs.GetJsonDump(), 
            "application/octet-stream", 
            $"backup-{Clock.Now.Timestamp()}.zip");

    //// POST api/BackupRestore
    //[HttpPost]
    //public void Post([FromBody]IFormFile backupFile)
    //{

    //}
}