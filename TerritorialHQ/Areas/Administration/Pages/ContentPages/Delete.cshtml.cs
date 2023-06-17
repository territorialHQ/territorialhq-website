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

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly ContentPageService _service;

        public DeleteModel(ContentPageService service)
        {
            _service = service;
        }

        public DTOContentPage? ContentPage { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContentPage = await _service.FindAsync<DTOContentPage>("ContentPage", id);

            if (ContentPage == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult>
            OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!(await _service.Remove("ContentPage", id)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
