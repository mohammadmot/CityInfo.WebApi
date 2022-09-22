using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        FileExtensionContentTypeProvider _FileExtension;
        public FilesController(FileExtensionContentTypeProvider FileExtension) // inject dependency in class constructor
        {
            _FileExtension = FileExtension;
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var filePath = "Google_Drive_logo.png"; // "Google_Drive_logo.rar"; //  "https://ar.m.wikipedia.org/wiki/%D9%85%D9%84%D9%81:Google_Drive_logo.png";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // check file content type
            if (!_FileExtension.TryGetContentType(filePath,
                out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType /*"text/plain"*/, Path.GetFileName(filePath));
        }
    }
}
