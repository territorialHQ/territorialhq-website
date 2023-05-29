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
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LoggerService _loggerService;

        public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, LoggerService loggerService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _loggerService = loggerService;
        }


        public IdentityUser IUser { get; set; }

        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        [Display(Name = "Assign to role:")]
        public string RoleId { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            UserId = id;
            IUser = await _userManager.FindByIdAsync(id);

            if (IUser == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(IUser);
            ViewData["RoleId"] = new SelectList(_roleManager.Roles, "Name", "Name", userRoles.FirstOrDefault()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            IUser = await _userManager.FindByIdAsync(UserId);
            if (IUser == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(IUser);
            for (int i = 0; i < userRoles.Count; i++)
                await _userManager.RemoveFromRoleAsync(IUser, userRoles[i]);

            await _userManager.AddToRoleAsync(IUser, RoleId);
            await _userManager.UpdateSecurityStampAsync(IUser);

            _loggerService.Log.Information("{User} added {TargetUser} to role {Role}", User.Identity.Name, IUser.UserName, RoleId);

            return RedirectToPage("./Index");
        }

    }
}
