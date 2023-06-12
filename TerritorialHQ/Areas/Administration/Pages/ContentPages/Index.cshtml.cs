using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ApisService _service;

        public IndexModel(ApisService service)
        {
            _service = service;
        }

        public IList<ContentPage>? ContentPage { get; set; }

        public async Task OnGetAsync()
        {
            ContentPage = await _service.GetAllAsync<ContentPage>("ContentPage");
        }
    }
}
