using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.X500;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Models.ViewComponents
{
    public class ClanLeaderboardViewComponent : ViewComponent
    {
        private readonly ClanService _clanService;
        public ClanLeaderboardViewComponent(ClanService clanService)
        {
            _clanService = clanService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _clanService.GetTopClans(10);
            return View(model);
        }
    }
}
