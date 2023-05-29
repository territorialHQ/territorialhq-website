using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class DetailsModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ClanService _service;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DiscordBotService _discordBotService;

        public DetailsModel(IMapper mapper, LoggerService logger, ClanService service, UserManager<IdentityUser> userManager, DiscordBotService discordBotService)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _userManager = userManager;
            _discordBotService = discordBotService;
        }

        public Clan Clan { get; set; }

        public List<ClanUserRelation> UserRelations { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clan = await _service.FindAsync(id);

            if (Clan == null)
            {
                return NotFound();
            }

            UserRelations = Clan.ClanUserRelations ?? new List<ClanUserRelation>();

            await FillStaffUserSelect();

            return Page();
        }

        public async Task<IActionResult> OnPostAddUser(string id, string userId)
        {
            Clan = await _service.FindAsync(id);

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (!Clan.ClanUserRelations.Any(r => r.UserId == user.Id))
                {
                    Clan.ClanUserRelations.Add(new ClanUserRelation() { UserId = user.Id });
                    _service.Update(Clan);

                    await _service.SaveChangesAsync(User);
                }
            }
            UserRelations = Clan.ClanUserRelations ?? new List<ClanUserRelation>();

            await FillStaffUserSelect();

            return Page();
        }


        public async Task<IActionResult> OnPostRemoveUser(string id, string userId)
        {
            Clan = await _service.FindAsync(id);

            Clan.ClanUserRelations.RemoveAll(r => r.UserId == userId);
            _service.Update(Clan);
            await _service.SaveChangesAsync(User);
            UserRelations = Clan.ClanUserRelations ?? new List<ClanUserRelation>();

            await FillStaffUserSelect();

            return Page();
        }

        public async Task<IActionResult> OnPostMarkForReview(string id)
        {
            Clan = await _service.FindAsync(id);

            Clan.InReview = true;
            _service.Update(Clan);
            await _service.SaveChangesAsync(User);

            await _discordBotService.SendReviewNotificationAsync(User.Identity.Name, id);

            return RedirectToPage("./Details", new { id = id });
        }

        public async Task<IActionResult> OnPostPublish(string id)
        {
            Clan = await _service.FindAsync(id);

            if (User.IsInRole("Administrator"))
            {
                Clan.InReview = false;
                Clan.IsPublished = true;
                _service.Update(Clan);
                await _service.SaveChangesAsync(User);
            }

            await FillStaffUserSelect();

            return Page();
        }

        private async Task FillStaffUserSelect()
        {
            if (User.IsInRole("Administrator"))
            {
                var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
                staffUsers = staffUsers.Except(UserRelations.Select(s => s.User)).ToList();

                ViewData["UserId"] = new SelectList(staffUsers, "Id", "UserName");
            }
        }
    }
}
