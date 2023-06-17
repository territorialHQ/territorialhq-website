using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly ClanService _service;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(ClanService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [BindProperty]
        public DTOClan? Clan { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Clan = await _service.FindAsync<DTOClan>("Clan", id);

            if (Clan == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult>
            OnPostAsync(string id)
        {
            if (id == null)
                return NotFound();

            var item = await _service.FindAsync<DTOClan>("Clan", id);
            if (item == null)
                return NotFound();

            if (item.LogoFile != null)
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.LogoFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }
            if (item.BannerFile != null)
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.BannerFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }

            if (!(await _service.Remove("Clan", id)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
