using System.IO;
using System.Threading.Tasks;

using Brotal.Extensions;

using Business;

using Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages
{
    public class BackupRestoreModel : PageModel
    {
        private readonly BackupRestoreService _brs;

        public BackupRestoreModel(
            BackupRestoreService brs)
        {
            _brs = brs;
        }

        public Stream Data;

        public async Task<IActionResult> OnGet()
        {
            Data = await _brs.GetJsonDump();

            //return File(
            //    mr, 
            //    "application/force-download", 
            //    $"backup{Clock.Now.Timestamp()}.json"
            //);

            return File(
                Data,
                "application/octet-stream",
                $"backup-{Clock.Now.Timestamp()}.zip"
            );
        }
    }
}