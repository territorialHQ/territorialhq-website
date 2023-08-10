using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.CommunityEvents
{
    [Authorize(Roles = "Administrator, Staff")]
    public class IndexModel : PageModel
    {
        private readonly CommunityEventService _service;

        public IndexModel(CommunityEventService service)
        {
            _service = service;
        }

        public List<DTOCommunityEvent>? CommunityEvents { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            CommunityEvents = await _service.GetAllAsync<DTOCommunityEvent>("CommunityEvent");

            return Page();
        }
    }
}
