using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentCreators
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly ContentCreatorService _service;

        public DeleteModel(ContentCreatorService service)
        {
            _service = service;
        }

        public DTOContentCreator? ContentCreator { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContentCreator = await _service.FindAsync<DTOContentCreator>("ContentCreator", id);

            if (ContentCreator == null)
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

            if (!(await _service.Remove("ContentCreator", id)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
