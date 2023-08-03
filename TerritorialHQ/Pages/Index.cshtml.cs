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
        private readonly ContentCreatorService _contentCreatorService;

        public List<DTOJournalArticleListEntry>? JournalArticles { get; set; }
        public List<DTOContentCreator>? ContentCreators { get; set; }

        public IndexModel(JournalArticleService journalArticleService, ContentCreatorService contentCreatorService)
        {
            _journalArticleService = journalArticleService;
            _contentCreatorService = contentCreatorService;
        }

        public async Task<IActionResult> OnGet()
        {
            JournalArticles = (await _journalArticleService.GetAllAsync<DTOJournalArticleListEntry>("JournalArticle/Listing"))?.OrderByDescending(o => o.IsSticky).ThenBy(o => o.PublishFrom).ToList() ?? new();
            ContentCreators = await _journalArticleService.GetAllAsync<DTOContentCreator>("ContentCreator");

            return Page();
        }
    }
}