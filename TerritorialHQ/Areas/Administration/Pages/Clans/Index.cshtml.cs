using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class IndexModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ClanService _service;

        public IndexModel(IMapper mapper, LoggerService logger, ClanService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public IList<Clan> Clan { get; set; }

        public async Task OnGetAsync()
        {
            Clan = await _service.GetAllAsync();
        }
    }
}
