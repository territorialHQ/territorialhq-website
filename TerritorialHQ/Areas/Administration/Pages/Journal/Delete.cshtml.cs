using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles ="Administrator, Journalist")]
    public class DeleteModel : PageModel
    {
        private readonly JournalArticleService _service;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(JournalArticleService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        public DTOJournalArticle? JournalArticle { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JournalArticle = await _service.FindAsync<DTOJournalArticle>("JournalArticle", id);

            if (JournalArticle == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult>
            OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync<DTOJournalArticle>("JournalArticle", id);
            if (item == null)
                return NotFound();

            if (item.Image != null)
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.Image);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }


            if (!(await _service.Remove("JournalArticle", id)))
                throw new Exception("Error while saving data set.");


            return RedirectToPage("./Index");
        }
    }
}
