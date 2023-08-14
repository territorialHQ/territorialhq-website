using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Models.ViewModels;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Areas.Administration.Pages.Home
{
    [Authorize(Roles = "Administrator, Staff, Journalist, Editor")]
    public class IndexModel : PageModel
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ClanService _clanService;

        public IndexModel(IMemoryCache memoryCache, ClanService clanService)
        {
            _memoryCache = memoryCache;
            _clanService = clanService;
        }

        public List<DTOClanListEntry>? ClansInReview { get; set; } = new();
        public List<LoginStat>? LoginStats { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Journalist") || User.IsInRole("Editor"))
                    return RedirectToPage("/Journal/Index");

                if (User.IsInRole("Staff"))
                    return RedirectToPage("/Clans/Index");
            }

            if (_memoryCache.TryGetValue("login-stats", out List<LoginStat>? logins))
            {
                if (logins != null)
                    logins.RemoveAll(s => s.Timestamp < DateTime.Now.AddDays(-1));

                _memoryCache.Set("login-stats", logins);

                LoginStats = logins;
            }

            ClansInReview = (await _clanService.GetClanListings("Clan/Listing") ?? new List<DTOClanListEntry>()).Where(c => c.InReview).ToList();

            return Page();
        }
    }
}
