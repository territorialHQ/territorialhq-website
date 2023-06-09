using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles ="Administrator, Journalist")]
    public class IndexModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ApisService _service;

        public IndexModel(IMapper mapper, LoggerService logger, ApisService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public List<JournalArticle> JournalArticle { get; set; }

        public async Task OnGetAsync()
        {
            JournalArticle = await _service.GetAllAsync<JournalArticle>("JournalArticle");
        }
    }
}
