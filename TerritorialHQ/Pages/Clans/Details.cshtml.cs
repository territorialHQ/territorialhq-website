using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Pages.Clans
{
    public class DetailsModel : PageModel
    {
        private readonly ChartDataService _chartDataService;
        private readonly IMemoryCache _memoryCache;
        private readonly ClanService _clanService;

        public DetailsModel(ChartDataService chartDataService, IMemoryCache memoryCache, ClanService clanService)
        {
            _chartDataService = chartDataService;
            _memoryCache = memoryCache;
            _clanService = clanService;
        }

        public DTOClan? Clan { get; set; }

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