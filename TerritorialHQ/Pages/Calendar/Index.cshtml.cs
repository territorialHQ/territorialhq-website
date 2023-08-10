using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Pages.Calendar
{
    public class IndexModel : PageModel
    {
        private readonly CommunityEventService _communityEventService;

        public IndexModel(CommunityEventService communityEventService)
        {
            _communityEventService = communityEventService;
        }

        public int Days { get; set; } = 14;
        public List<DTOCommunityEvent> CommunityEvents { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            CommunityEvents = await _communityEventService.GetSchedule(Days) ?? new();

            return Page();
        }
    }
}
