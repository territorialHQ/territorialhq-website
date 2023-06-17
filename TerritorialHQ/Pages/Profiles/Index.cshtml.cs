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
        private readonly ApisService _apisService;
        private readonly AppUserService _appUserService;
        private readonly BotEndpointService _botEndpointService;

        public IndexModel(ApisService apisService, AppUserService appUserService, BotEndpointService botEndpointService)
        {
            _apisService = apisService;
            _appUserService = appUserService;
            _botEndpointService = botEndpointService;
        }

        public Dictionary<string, uint> ClanPoints = new();
        public bool IsPrivate { get; set; }
        public bool IsOwnProfile { get; set; }
        public DTOAppUser? AppUser { get; set; }

        public async Task<IActionResult> OnGet(string? id)
        {
            //if (id == null)
            //    return NotFound();

            //AppUser = await _appUserService.FindAsync<DTOAppUser>("AppUser", id);
            //if (User.Identity?.Name != id)
            //{
            //    if (AppUser == null/* || !AppUser.*/)
            //        IsPrivate = true;
            //}
            //else
            //{
            //    IsOwnProfile = true;
            //}

            //if (!IsPrivate)
            //{
            //    var clans = await _apisService.GetAllAsync<Clan>("Clan") ?? new List<Clan>();
            //    var clansWithEndpoints = clans.Where(c => !string.IsNullOrEmpty(c.BotEndpoint));

            //    foreach (var clan in clansWithEndpoints)
            //    {
            //        uint points = await _botEndpointService.GetPlayerPoints(clan.BotEndpoint, id);
            //        if (!ClanPoints.ContainsKey(clan.Name!))
            //            ClanPoints.Add(clan.Name!, points);
            //    }
            //}

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var appUser = await _appUserService.FindAsync<DTOAppUser>("AppUser", User.Identity?.Name);
            //if (appUser == null) 
            //    return NotFound();

            //appUser.Public = !appUser.Public;

            //if (!(await _appUserService.Update<DTOAppUser>("AppUser", appUser)))
            //    throw new Exception("Error while updating profile settings");

            return RedirectToPage("./Index"/*, new { id = appUser.DiscordId }*/);
        }
    }
}
