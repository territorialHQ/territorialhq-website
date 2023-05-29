using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TerritorialHQ.Pages.Installation.Initialize
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,
                          IUserStore<IdentityUser> userStore)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
        }

        public async Task<IActionResult> OnGet()
        {
            // If there is already at least one user in the database, initialization can no longer be done. Hide the page.
            if (_userManager.Users.Count() > 0)
                return NotFound();

            // Check if the default roles have already been created. If not, create them.
            if (_roleManager.Roles.Count() == 0)
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Administrator",
                    NormalizedName = "Administrator"
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Staff",
                    NormalizedName = "Staff"
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Journalist",
                    NormalizedName = "Journalist"
                });
            }

            return Page();
        }

        [BindProperty]
        [Required]
        [MinLength(16)]
        [Display(Name = "Initial Admin Discord Id")]
        public string UserId { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, UserId, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, "dummy@dummy.com", CancellationToken.None);
            await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);

            var userCreateResult = await _userManager.CreateAsync(user);
            if (userCreateResult.Succeeded)
            {

                var role = await _roleManager.FindByNameAsync("Administrator");
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, "Administrator");
                    await _userManager.AddLoginAsync(user, new UserLoginInfo("Discord", UserId, "Discord"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Administrator role could not be found.");
                }
            }
            foreach (var error in  userCreateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (!ModelState.IsValid) {
                return Page();
            }

            return Redirect("/");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }

    }


}
