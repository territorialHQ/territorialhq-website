using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class IndexModel : PageModel
    {
        private readonly ApisService _service;

        public IndexModel(ApisService service)
        {
            _service = service;
        }

        public List<Clan>? Clan { get; set; }

        public async Task OnGetAsync()
        {
            Clan = await _service.GetAllAsync<Clan>("Clan");
        }
    }
}
