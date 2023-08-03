using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Pages.Profiles
{
    public class IndexModel : PageModel
    {
        private readonly ClanService _clanService;
        private readonly AppUserService _appUserService;
        private readonly BotEndpointService _botEndpointService;

        public IndexModel(ClanService clanService, AppUserService appUserService, BotEndpointService botEndpointService)
        {
            _clanService = clanService;
            _appUserService = appUserService;
            _botEndpointService = botEndpointService;
        }

        public Dictionary<DTOClan, uint> ClanPoints = new();
        public bool IsPrivate { get; set; }
        public bool IsOwnProfile { get; set; }
        public DTOAppUser? AppUser { get; set; }

        public async Task<IActionResult> OnGet(string? id)
        {
            if (id == null)
                return NotFound();

            AppUser = await _appUserService.FindAsync<DTOAppUser>("AppUser", id);
            if (AppUser == null)
            {
                IsPrivate = true;
                return Page();
            }

            if (User.Identity?.Name != id)
            {
                if (!AppUser.Public)
                    IsPrivate = true;
            }
            else
            {
                IsOwnProfile = true;
            }

            if (!IsPrivate)
            {
                var clans = await _clanService.GetAllAsync<DTOClan>("Clan") ?? new List<DTOClan>();
                var clansWithEndpoints = clans.Where(c => !string.IsNullOrEmpty(c.BotEndpoint));

                foreach (var clan in clansWithEndpoints)
                {
                    uint points = await _botEndpointService.GetPlayerPoints(clan.BotEndpoint, id);
                    if (!ClanPoints.ContainsKey(clan))
                        ClanPoints.Add(clan, points);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var appUser = await _appUserService.FindAsync<DTOAppUser>("AppUser", User.Identity?.Name!);
            if (appUser == null)
                return NotFound();

            if (!(await _appUserService.TogglePublicAsync(appUser.Id!)))
                throw new Exception("Error while updating profile settings");

            return RedirectToPage("./Index", new { id = appUser.DiscordId });
        }
    }
}
