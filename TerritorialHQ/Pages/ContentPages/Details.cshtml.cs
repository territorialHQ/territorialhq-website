using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Pages.ContentPages
{
    public class DetailsModel : PageModel
    {
        private readonly NavigationEntryService _navigationEntryService;
        private readonly ContentPageService _contentPageService;

        public DetailsModel(NavigationEntryService navigationEntryService, ContentPageService contentPageService)
        {
            _navigationEntryService = navigationEntryService;
            _contentPageService = contentPageService;
        }

        public DTONavigationEntry? NavEntry { get; set; }
        public DTOContentPage? ContentPage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            // Read the URL to determine which content page the user wanted to see
            var pathArr = Request.Path.Value?.TrimEnd('/').Split("/");
            var slug = pathArr![^1];

            if (slug == null || slug.Length == 0)
                return NotFound();

            // See if the slug belongs to an actual navigation entry
            NavEntry = await _navigationEntryService.FindAsync<DTONavigationEntry>("NavigationEntry", slug);
            if (NavEntry == null)
                return NotFound();

            // Get the content page
            if (NavEntry.ContentPageId != null)
                ContentPage = await _contentPageService.FindAsync<DTOContentPage>("ContentPage", NavEntry.ContentPageId);

            return Page();
        }
    }
}
