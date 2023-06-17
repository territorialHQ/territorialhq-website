using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ContentPageService _service;

        public IndexModel(ContentPageService service)
        {
            _service = service;
        }

        public IList<DTOContentPage>? ContentPage { get; set; }

        public async Task OnGetAsync()
        {
            ContentPage = await _service.GetAllAsync<DTOContentPage>("ContentPage");
        }
    }
}
