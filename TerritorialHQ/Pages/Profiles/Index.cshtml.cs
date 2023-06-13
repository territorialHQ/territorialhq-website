using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Pages.Profiles
{
    public class IndexModel : PageModel
    {
        private readonly ApisService _apisService;
        private readonly BotEndpointService _botEndpointService;

        public IndexModel(ApisService apisService, BotEndpointService botEndpointService)
        {
            _apisService = apisService;
            _botEndpointService = botEndpointService;
        }

        public Dictionary<string, uint> ClanPoints = new();

        public async Task<IActionResult> OnGet(string? id)
        {
            if (id == null)
                return NotFound();

            var clans = await _apisService.GetAllAsync<Clan>("Clan") ?? new List<Clan>();
            var clansWithEndpoints = clans.Where(c => !string.IsNullOrEmpty(c.BotEndpoint));

            foreach (var clan in clansWithEndpoints)
            {
                uint points = await _botEndpointService.GetPlayerPoints(clan.BotEndpoint, id);
                if (!ClanPoints.ContainsKey(clan.Name!))
                    ClanPoints.Add(clan.Name!, points); 
            }

            return Page();
        }
    }
}
