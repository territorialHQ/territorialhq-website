using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class DetailsModel : PageModel
    {
        private readonly ClanService _service;
        private readonly AppUserService _userService;
        private readonly DiscordBotService _discordBotService;
        private readonly ClanRelationService _clanRelationService;

        public DetailsModel(ClanService service,  DiscordBotService discordBotService, AppUserService userService, ClanRelationService clanRelationService)
        {
            _service = service;
            _discordBotService = discordBotService;
            _userService = userService;
            _clanRelationService = clanRelationService;
        }

        public DTOClan? Clan { get; set; }
        public List<ClanUserRelation>? UserRelations { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Clan = await _service.FindAsync<DTOClan>("Clan", id);
            if (Clan == null)
                return NotFound();

            await FillStaffUserSelect();
            await FillRelationSelects();

            return Page();
        }

        public async Task<IActionResult> OnPostAddUser(string id, string userId)
        {
            if (userId != null)
            {
                var relation = new DTOClanUserRelation()
                {
                    ClanId = id,
                    AppUserId = userId
                };

                if (await _service.Add<DTOClanUserRelation>("ClanUserRelation", relation) == null)
                    throw new Exception("Error while saving relation data set.");
            }

            return RedirectToPage("./Details", new { id });
        }


        public async Task<IActionResult> OnPostRemoveUser(string id, string relationId)
        {
            if (!(await _service.Remove("ClanUserRelation", relationId)))
                throw new Exception("Error while saving relation data set.");

            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnPostMarkForReview(string? id)
        {
            Clan = await _service.FindAsync<DTOClan>("Clan", id!);
            if (Clan == null)
                return NotFound();

            Clan.InReview = true;

            if (!(await _service.Update<DTOClan>("Clan", Clan)))
                throw new Exception("Error while saving data set.");

            await _discordBotService.SendClanReviewNotificationAsync(User.Identity?.Name, id);

            await FillStaffUserSelect();
            await FillRelationSelects();

            return Page();
        }

        public async Task<IActionResult> OnPostPublish(string id)
        {
            Clan = await _service.FindAsync<DTOClan>("Clan", id);
            if (Clan == null)
                return NotFound();

            if (User.IsInRole("Administrator"))
            {
                Clan.InReview = false;
                Clan.IsPublished = true;

                if (!(await _service.Update<DTOClan>("Clan", Clan)))
                    throw new Exception("Error while saving data set.");
            }

            await FillStaffUserSelect();
            await FillRelationSelects();

            return Page();
        }

        public async Task<IActionResult> OnPostAddRelation(string id, string targetClanId, DiplomaticRelationType relationType)
        {
            if (id == null) 
                return NotFound();

            if (targetClanId != null)
            {
                var relation = new DTOClanRelation() { ClanId = id, TargetClanId = targetClanId, DiplomaticRelation = relationType };

                if (await _clanRelationService.Add<DTOClanRelation>("ClanRelation", relation) == null)
                    throw new Exception("Error while saving data set.");

            }

            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnPostRemoveRelation(string id, string clanId)
        {
            if (id == null)
                return NotFound();

            if (!(await _clanRelationService.Remove("ClanRelation", id)))
                throw new Exception("Error while saving relation data set.");

            return RedirectToPage("./Details", new { id = clanId });
        }

        private async Task FillStaffUserSelect()
        {
            if (User.IsInRole("Administrator"))
            {
                var assignedStaffUsersIds = Clan?.AssignedAppUsers.Select(s => s.AppUserId).ToList() ?? new List<string?>();
                var staffUsers = await _userService.GetUsersInRoleAsync(AppUserRole.Staff);

                staffUsers!.RemoveAll(u => assignedStaffUsersIds.Contains(u.Id));

                ViewData["UserId"] = new SelectList(staffUsers, "Id", "UserName");
            }
        }

        private async Task FillRelationSelects()
        {
            List<DTOClanListEntry> clans = await _service.GetClanListings("Clan/Listing") ?? new();

            clans.RemoveAll(c => Clan!.ClanRelations.Any(r => r.TargetClanId == c.Id) || c.Id == Clan.Id);

            ViewData["TargetClanId"] = new SelectList(clans.Select(c => new { Name = c.Tag, Id = c.Id }).OrderBy(o => o.Name), "Id", "Name");
        }
    }
}
