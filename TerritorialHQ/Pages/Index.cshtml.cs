using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Pages
{
    public class IndexModel : PageModel
    {
        private readonly JournalArticleService _journalArticleService;

        public List<DTOJournalArticleListEntry> JournalArticles { get; set; }

        public IndexModel(JournalArticleService journalArticleService)
        {
           _journalArticleService = journalArticleService;
        }

        public async Task<IActionResult> OnGet()
        {
            JournalArticles = (await _journalArticleService.GetAllAsync<DTOJournalArticleListEntry>("JournalArticle/Listing"))?.OrderByDescending(o => o.IsSticky).ThenBy(o => o.PublishFrom).ToList() ?? new();

            return Page();
        }
    }
}