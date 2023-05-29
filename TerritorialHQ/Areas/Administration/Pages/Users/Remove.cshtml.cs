using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Users
{
    [Authorize(Roles ="Administrator")]
    public class RemoveModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LoggerService _loggerService;


        public RemoveModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, LoggerService loggerService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _loggerService = loggerService;
        }


        public async Task<IActionResult> OnPost(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var removedRoles = String.Join(',', userRoles);
            for (int i = 0; i < userRoles.Count; i++)
                await _userManager.RemoveFromRoleAsync(user, userRoles[i]);

            await _userManager.UpdateSecurityStampAsync(user);

            _loggerService.Log.Information("{User} removed {TargetUser} from roles {Roles}", User.Identity.Name, user.UserName, removedRoles);

            return RedirectToPage("./Index");
        }

    }
}
