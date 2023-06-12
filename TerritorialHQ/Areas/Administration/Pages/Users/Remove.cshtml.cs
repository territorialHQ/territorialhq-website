using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Users
{
    [Authorize(Roles ="Administrator")]
    public class RemoveModel : PageModel
    {
        private readonly AppUserService _apisUserService;

        public RemoveModel(AppUserService apisUserService)
        {
            _apisUserService = apisUserService;
        }


        public async Task<IActionResult> OnPost(string id)
        {
            var user = await _apisUserService.FindAsync<AppUser>("AppUser", id);
            if (user == null)
                return NotFound();

            user.Role = null;

            if (!(await _apisUserService.Update<AppUser>("AppUser", user)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }

    }
}
