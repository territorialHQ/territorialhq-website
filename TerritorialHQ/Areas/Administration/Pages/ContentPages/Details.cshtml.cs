using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class DetailsModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ContentPageService _service;

        public DetailsModel(IMapper mapper, LoggerService logger, ContentPageService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public ContentPage ContentPage { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContentPage = await _service.FindAsync(id);

            if (ContentPage == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
