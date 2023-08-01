using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Pages.Journal
{
    public class DetailsModel : PageModel
    {
        private readonly JournalArticleService _journalArticleService;

        public DetailsModel(JournalArticleService journalArticleService)
        {
            _journalArticleService = journalArticleService;
        }

        public DTOJournalArticle? Article { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id  == null) 
                return NotFound();

            Article = await _journalArticleService.FindAsync<DTOJournalArticle>("JournalArticle", id);
            if (Article == null)
                return NotFound();

            return Page();
        }
    }
}
