using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Numerics;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;

namespace TerritorialHQ.Pages.Ajax
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class UploadsModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly LoggerService _logger;

        public UploadsModel(IWebHostEnvironment env, LoggerService logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync([FromForm] IFormFile upload)
        {

            if (upload != null)
            {
                try
                {
                    var filename = await ImageHelper.ProcessImage(upload, _env.WebRootPath + "/Data/Uploads/Pages/", false, null, false);
                    var response = new { url = "/Data/Uploads/Pages/" + filename };

                    _logger.Log.Information("{User} uploaded file {File} with CKEditor.", User.Identity.Name, filename);

                    return new JsonResult(response);
                }
                catch (Exception ex)
                {
                    var response = new { error = ex.Message };
                    return new JsonResult(response);
                }
            }

            return Forbid();
        }
    }
}
