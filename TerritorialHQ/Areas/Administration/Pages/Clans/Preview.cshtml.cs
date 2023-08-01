using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    public class PreviewModel : PageModel
    {
        private readonly ClanService _clanService;

        public PreviewModel(ClanService clanService)
        {
            _clanService = clanService;
        }

        public DTOClan? Clan { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            Clan = await _clanService.FindAsync<DTOClan>("Clan", id);
            if (Clan == null)
                return NotFound();

            return Page();
        }
    }
}
