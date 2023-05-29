using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly NavigationEntryService _service;

        public DeleteModel(IMapper mapper, LoggerService logger, NavigationEntryService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        [BindProperty]
        public NavigationEntry NavigationEntry { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NavigationEntry = await _service.FindAsync(id);

            if (NavigationEntry == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult>
            OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _service.RemoveAsync(id);
            await _service.SaveChangesAsync(User);


            return RedirectToPage("./Index");
        }
    }
}
