using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly NavigationEntryService _service;

        public DeleteModel(NavigationEntryService service)
        {
            _service = service;
        }

        [BindProperty]
        public DTONavigationEntry? NavigationEntry { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NavigationEntry = await _service.FindAsync<DTONavigationEntry>("NavigationEntry", id);

            if (NavigationEntry == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(await _service.Remove("NavigationEntry", id)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
