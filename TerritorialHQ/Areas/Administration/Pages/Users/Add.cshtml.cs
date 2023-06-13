using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Users
{
    [Authorize(Roles = "Administrator")]
    public class AddModel : PageModel
    {

        private readonly AppUserService _userService;

        public AddModel(AppUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnPostAsync(string userId, AppUserRole role)
        {
            var user = await _userService.FindAsync<AppUser>("AppUser", userId);
            if (user == null)
                return NotFound();

            if (user.Role != null)
                throw new Exception("User is already in a role, cannot assign a new one.");

            user.Role = role;

            if (!(await _userService.Update("AppUser", user)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
