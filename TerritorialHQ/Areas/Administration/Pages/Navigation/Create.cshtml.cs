using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly NavigationEntryService _service;
        private readonly ContentPageService _contentPageService;

        public CreateModel(
            IMapper mapper,
            NavigationEntryService service,
            ContentPageService contentPageService)
        {
            _mapper = mapper;
            _service = service;
            _contentPageService = contentPageService;
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

            ViewData["ContentPageId"] = new SelectList(await _contentPageService.GetAllAsync<DTOContentPage>("ContentPage"), "Id", "DisplayName", this.ContentPageId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ContentPageId"] = new SelectList(await _contentPageService.GetAllAsync<DTOContentPage>("ContentPage"), "Id", "DisplayName", this.ContentPageId);
                return Page();
            }

            var item = new DTONavigationEntry();
            _mapper.Map(this, item);

            if (!(await _service.Add<DTONavigationEntry>("NavigationEntry", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
