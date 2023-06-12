using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ApisService _service;

        public IndexModel(ApisService service)
        {
            _service = service;
        }

        public IList<NavigationEntry>? NavigationEntries { get; set; }

        public async Task OnGetAsync()
        {
            NavigationEntries = await _service.GetAllAsync<NavigationEntry>("NavigationEntry");
        }
    }
}
