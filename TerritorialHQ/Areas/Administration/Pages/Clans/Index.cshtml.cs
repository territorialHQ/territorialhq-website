using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class IndexModel : PageModel
    {
        private readonly ClanService _service;

        public IndexModel(ClanService service)
        {
            _service = service;
        }

        public List<DTOClan>? Clans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Clans = await _service.GetAllAsync<DTOClan>("Clan");

            return Page();
        }
    }
}
