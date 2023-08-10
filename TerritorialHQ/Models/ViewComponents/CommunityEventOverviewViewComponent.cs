using Microsoft.AspNetCore.Mvc;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Models.ViewComponents
{
    public class CommunityEventOverviewViewComponent : ViewComponent
    {
        private readonly CommunityEventService _communityEventService;

        public CommunityEventOverviewViewComponent(CommunityEventService communityEventService)
        {
            _communityEventService = communityEventService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int days = 7)
        {
            var model = await _communityEventService.GetSchedule(days);
            return View(model);
        }
    }
}
