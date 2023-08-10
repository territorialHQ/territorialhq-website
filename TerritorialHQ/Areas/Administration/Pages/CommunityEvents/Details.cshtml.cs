using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.CommunityEvents
{
    [Authorize(Roles = "Administrator, Staff")]
    public class DetailsModel : PageModel
    {
        private readonly CommunityEventService _service;
        private readonly DiscordBotService _discordBotService;

        public DetailsModel(CommunityEventService service, DiscordBotService discordBotService)
        {
            _service = service;
            _discordBotService = discordBotService;
        }

        public DTOCommunityEvent? CommunityEvent { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
                return NotFound();

            CommunityEvent = await _service.FindAsync<DTOCommunityEvent>("CommunityEvent", id);
            if (CommunityEvent == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostMarkForReview(string? id)
        {
            CommunityEvent = await _service.FindAsync<DTOCommunityEvent>("CommunityEvent", id!);
            if (CommunityEvent == null)
                return NotFound();

            CommunityEvent.InReview = true;

            if (!(await _service.Update<DTOCommunityEvent>("CommunityEvent", CommunityEvent)))
                throw new Exception("Error while saving data set.");

            await _discordBotService.SendEventReviewNotificationAsync(User.Identity?.Name, id);

            return Page();
        }

        public async Task<IActionResult> OnPostPublish(string id)
        {
            CommunityEvent = await _service.FindAsync<DTOCommunityEvent>("CommunityEvent", id!);
            if (CommunityEvent == null)
                return NotFound();

            if (User.IsInRole("Administrator"))
            {
                CommunityEvent.InReview = false;
                CommunityEvent.IsPublished = true;

                if (!(await _service.Update<DTOCommunityEvent>("CommunityEvent", CommunityEvent)))
                    throw new Exception("Error while saving data set.");
            }

            return Page();
        }
    }
}
