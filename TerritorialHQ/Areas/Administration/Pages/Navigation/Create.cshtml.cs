using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly NavigationEntryService _service;
        private readonly ContentPageService _contentpageService;

        public CreateModel(
            IMapper mapper, LoggerService logger,
            NavigationEntryService service,
            ContentPageService contentpageService
        )
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _contentpageService = contentpageService;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Display Name (required)")]
        public string? Name { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Navigation Slug (required)")]
        public string? Slug { get; set; }
        [BindProperty]
        [Display(Name = "Public")]
        public bool Public { get; set; }
        [BindProperty]
        [Display(Name = "Order")]
        public short Order { get; set; }

        [BindProperty]
        public string? ParentId { get; set; }
        [BindProperty]
        [Display(Name = "Linked Content Page")]
        public string? ContentPageId { get; set; }
        [BindProperty]
        [Display(Name = "Link to External URL")]
        public string? ExternalUrl { get; set; }

        public async Task<IActionResult> OnGet(string? parentId)
        {
            ParentId = parentId;

            ViewData["ContentPageId"] = new SelectList(await _contentpageService.GetAllAsync(), "Id", "DisplayName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ContentPageId"] = new SelectList(await _contentpageService.GetAllAsync(), "Id", "DisplayName");
                return Page();
            }

            var item = new NavigationEntry();
            _mapper.Map(this, item);

            _service.Add(item);
            await _service.SaveChangesAsync(User);

            return RedirectToPage("./Index");
        }
    }
}
