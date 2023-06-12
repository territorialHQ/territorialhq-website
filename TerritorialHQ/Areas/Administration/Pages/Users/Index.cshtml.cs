using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Services;
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

        public Dictionary<AppUserRole, List<AppUser>> UsersInRoles { get; set; } = new();

        [BindProperty]
        [MinLength(16)]
        [Display(Name = "Search for Discord ID")]
        public string? UserQuery { get; set; }

        public AppUser? QueryResult { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await GetCurrentUsers();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(UserQuery))
            {
                QueryResult = await _userService.FindAsync<AppUser>("AppUser", UserQuery);
            }

            await GetCurrentUsers();
            return Page();
        }

        private async Task GetCurrentUsers()
        {
            foreach (var role in (AppUserRole[])Enum.GetValues(typeof(AppUserRole)))
            {
                var users = await _userService.GetUsersInRoleAsync(role) ?? new List<AppUser>();
                if (users.Count > 0)
                {
                    UsersInRoles.Add(role, users);
                }
            }
        }
    }
}
