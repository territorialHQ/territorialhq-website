using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TerritorialHQ.Areas.Administration.Pages.Users
{
    [Authorize(Roles ="Administrator")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Dictionary<string, IList<IdentityUser>> UsersWithRoles { get; set; } = new();

        [BindProperty]
        [MinLength(16)]
        [Display(Name = "Search for Discord ID")]
        public string? UserQuery { get; set; }

        public IdentityUser QueryResult { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await GetCurrentUsers();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!string.IsNullOrEmpty(UserQuery))
            {
                QueryResult = await _userManager.FindByNameAsync(UserQuery);
            }

            await GetCurrentUsers();
            return Page();
        }

        private async Task GetCurrentUsers()
        {
            var roles = _roleManager.Roles.ToList();

            foreach (var role in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);
                if (users.Count > 0)
                {
                    UsersWithRoles.Add(role.Name, users);
                }
            }
        }
    }
}
