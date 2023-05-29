using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly NavigationEntryService _service;

        public IndexModel(IMapper mapper, LoggerService logger, NavigationEntryService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public IList<NavigationEntry> NavigationEntries { get; set; }

        public async Task OnGetAsync()
        {
            NavigationEntries = await _service.GetTopLevelAsync();
        }
    }
}
