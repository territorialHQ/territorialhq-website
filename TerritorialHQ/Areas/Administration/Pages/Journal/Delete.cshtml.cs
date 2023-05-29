using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles ="Administrator, Journalist")]
    public class DeleteModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly JournalArticleService _service;

        public DeleteModel(IMapper mapper, LoggerService logger, JournalArticleService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        [BindProperty]
        public JournalArticle JournalArticle { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JournalArticle = await _service.FindAsync(id);

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

            await _service.RemoveAsync(id);
            await _service.SaveChangesAsync(User);


            return RedirectToPage("./Index");
        }
    }
}
