using Business.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Policy = "UserRights")]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IFileService _fs;

        public FilesController(
            IFileService fs) =>
            _fs = fs;

        // GET: api/Files/user.png
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var stream = _fs.Get(id);
            var file = File(
                    stream,
                    "application/octet-stream",
                    id
                    );
            //stream.Close();
            //stream.Dispose();
            return file;
        }
    }
}
