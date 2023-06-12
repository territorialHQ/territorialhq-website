using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly ApisService _service;

        public DetailsModel(ApisService service)
        {
            _service = service;
        }

        public ContentPage? ContentPage { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContentPage = await _service.FindAsync<ContentPage>("ContentPage", id);

            if (ContentPage == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
