using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Models.ViewModels;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Pages.Clans
{
    public class DetailsModel : PageModel
    {
        private readonly ChartDataService _chartDataService;
        private readonly IMemoryCache _memoryCache;
        private readonly ClanService _clanService;
        private readonly BotEndpointService _botEndpointService;

        public DetailsModel(ChartDataService chartDataService, IMemoryCache memoryCache, ClanService clanService, BotEndpointService botEndpointService)
        {
            _chartDataService = chartDataService;
            _memoryCache = memoryCache;
            _clanService = clanService;
            _botEndpointService = botEndpointService;
        }

        public DTOClan? Clan { get; set; }
        public DiscordServerInfo? ServerInfo { get; set; }
        public LeaderboardEntry leaderboardEntry { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null) 
                return NotFound();  

            Clan = await _clanService.FindAsync<DTOClan>("Clan", id);
            if (Clan == null) 
                return NotFound();
            
            if (!Clan.IsPublished && 
                !User.IsInRole("Administrator") && 
                (!User.IsInRole("Staff") || (User.IsInRole("Staff") && !Clan.AssignedAppUsers.Any(u => u.AppUserName == User.Identity?.Name))))
            {
                return NotFound();
            }

            if (Clan.DiscordLink != null)
            {
                var lastSlash = Clan.DiscordLink.LastIndexOf('/');
                var inviteCode = Clan.DiscordLink.Substring(lastSlash).Replace("/", "");

                ServerInfo = await _botEndpointService.GetServerStats(inviteCode);
            }

            decimal points = await _clanService.GetPointsAtRank(250);

            leaderboardEntry = await _clanService.GetLeaderboardEntry(id) ?? new LeaderboardEntry();
            leaderboardEntry.RankThreshold = points;

            return Page();
        }

        public async Task<IActionResult> OnGetChartData(string clan)
        {

            List<ChartDataViewModel>? result = null;
            if (_memoryCache.TryGetValue("cg-" + clan, out List<ChartDataViewModel>? cachedResult))
            {
                result = cachedResult;
            }
            else
            {
                result = new List<ChartDataViewModel>();
                var data = await _chartDataService.GetClanChartData(clan);

                for (int i = 0; i < data.Count; i++)
                {
                    result.Add(new ChartDataViewModel() { Date = DateTime.Today.AddDays(i - data.Count).ToString("d"), Value = data[i] });
                }

                _memoryCache.Set("cg-" + clan, result);
            }

            return new JsonResult(result);
        }

        private class ChartDataViewModel
        {
            public string? Date { get; set; }
            public float Value { get; set; }
        }
    }
}
