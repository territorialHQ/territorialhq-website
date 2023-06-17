using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Users
{
    [Authorize(Roles ="Administrator")]
    public class IndexModel : PageModel
    {
        private readonly AppUserService _userService;

        public IndexModel(AppUserService userService)
        {
            _userService = userService;
        }

        public Dictionary<AppUserRole, List<DTOAppUser>> UsersInRoles { get; set; } = new();

        [BindProperty]
        [MinLength(16)]
        [Display(Name = "Search for Discord ID")]
        public string? UserQuery { get; set; }

        public DTOAppUser? QueryResult { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await GetCurrentUsers();
            return Page();
        }

        public async Task<IActionResult> OnPostSearch()
        {
            if (!string.IsNullOrEmpty(UserQuery))
            {
                QueryResult = await _userService.FindAsync<DTOAppUser>("AppUser", UserQuery);
            }

            await GetCurrentUsers();
            return Page();
        }

        public async Task<IActionResult> OnPostAddAppUser(string userId, AppUserRole role)
        {
            var user = await _userService.FindAsync<DTOAppUser>("AppUser", userId);
            if (user == null)
                return NotFound();

            if (user.Role != null)
                throw new Exception("User is already in a role, cannot assign a new one.");

            user.Role = role;

            if (!(await _userService.Update("AppUser", user)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostRemoveAppUser(string userId)
        {

            var user = await _userService.FindAsync<DTOAppUser>("AppUser", userId);
            if (user == null)
                return NotFound();

            await GetCurrentUsers();
            if (UsersInRoles[AppUserRole.Administrator].Count == 1 && user.Role == AppUserRole.Administrator)
                throw new Exception("Cannot remove last remaining administrator.");

           user.Role = null;

            if (!(await _userService.Update<DTOAppUser>("AppUser", user)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }

        private async Task GetCurrentUsers()
        {
            foreach (var role in (AppUserRole[])Enum.GetValues(typeof(AppUserRole)))
            {
                var users = await _userService.GetUsersInRoleAsync(role) ?? new List<DTOAppUser>();
                if (users.Count > 0)
                {
                    UsersInRoles.Add(role, users);
                }
            }
        }
    }
}
