using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace CityInfo.Api.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private FileExtensionContentTypeProvider _FileExtension;
        private readonly ILogger<FilesController> _logger;

        // constructor injection (for dependency injection)
        public FilesController(
            FileExtensionContentTypeProvider FileExtension, // inject dependency in class constructor
            ILogger<FilesController> Logger
            )
        {
            _FileExtension = FileExtension;
            // _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            try
            {
                #region log
                // just for test throw exception
                // - throw new Exception("My force exception ... :-)");

                if (_logger != null)
                    _logger.LogInformation("GetFile controller visited at {DT}", DateTime.Now.ToLongTimeString());

                System.Diagnostics.Debug.WriteLine($"Logging in output window ...");

                if (_logger != null)
                    _logger.LogInformation($"FilesController.GetFile with address {fileId}.");
                #endregion

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
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting FilesController.GetFile {fileId}", ex);
                return StatusCode(500, "A problem happen in file !!!");
            }
        }
    }
}
