using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles ="Administrator, Journalist, Editor")]
    public class IndexModel : PageModel
    {
        private readonly JournalArticleService _service;

        public IndexModel(JournalArticleService service)
        {
            _service = service;
        }

        public List<DTOJournalArticle>? JournalArticle { get; set; }

        public async Task OnGetAsync()
        {
            JournalArticle = await _service.GetAllAsync<DTOJournalArticle>("JournalArticle");
        }
    }
}
