using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly ContentPageService _service;

        public DetailsModel(ContentPageService service)
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
    }
}
