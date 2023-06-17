using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly NavigationEntryService _service;

        public IndexModel(NavigationEntryService service)
        {
            _service = service;
        }

        public IList<DTONavigationEntry>? NavigationEntries { get; set; }

        public async Task OnGetAsync()
        {
            NavigationEntries = await _service.GetAllAsync<DTONavigationEntry>("NavigationEntry");
        }
    }
}
