using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentCreators
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ContentCreatorService _service;

        public IndexModel(ContentCreatorService service)
        {
            _service = service;
        }

        public IList<DTOContentCreator>? ContentCreator { get; set; }

        public async Task OnGetAsync()
        {
            ContentCreator = await _service.GetAllAsync<DTOContentCreator>("ContentCreator");
        }
    }
}
