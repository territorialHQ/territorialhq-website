using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Pages.ContentPages
{
    public class DetailsModel : PageModel
    {
        private readonly ApisService _service;

        public DetailsModel(ApisService service)
        {
            _service = service;
        }

        public NavigationEntry? NavEntry { get; set; }
        public ContentPage? ContentPage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            // Read the URL to determine which content page the user wanted to see
            var pathArr = Request.Path.Value?.TrimEnd('/').Split("/");
            var slug = pathArr![^1];

            if (slug == null || slug.Length == 0)
                return NotFound();

            // See if the slug belongs to an actual navigation entry
            NavEntry = await _service.FindAsync<NavigationEntry>("NavigationEntry", slug);
            if (NavEntry == null) 
                return NotFound();

            // Get the content page
            if (NavEntry.ContentPageId != null)
                ContentPage = await _service.FindAsync<ContentPage>("ContentPage", NavEntry.ContentPageId);

            return Page();
        }
    }
}
