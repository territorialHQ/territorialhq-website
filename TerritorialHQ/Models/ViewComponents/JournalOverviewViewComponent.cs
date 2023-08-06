using Microsoft.AspNetCore.Mvc;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Models.ViewComponents
{
    public class JournalOverviewViewComponent : ViewComponent
    {
        private readonly JournalArticleService _journalArticleService;

        public JournalOverviewViewComponent(JournalArticleService journalArticleService)
        {
            _journalArticleService = journalArticleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? currentArticleId = null)
        {
            var model = await _journalArticleService.GetAllAsync<DTOJournalArticleListEntry>("JournalArticle/Listing");

            if (currentArticleId != null)
                model = model?.Where(a => a.Id != currentArticleId).OrderBy(o => o.PublishFrom).ToList();

            return View(model);
        }
    }
}
