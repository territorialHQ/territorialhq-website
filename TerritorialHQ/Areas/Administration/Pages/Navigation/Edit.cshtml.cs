using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Navigation
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly ApisService _service;

        public EditModel(
        IMapper mapper,
        ApisService service
        )
        {
            _mapper = mapper;
            _service = service;
        }


        [BindProperty]
        [Display(Name = " ")]
        public string Id { get; set; }
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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync<NavigationEntry>("NavigationEntry", id);
            if (item == null)
            {
                return NotFound();
            }

            _mapper.Map(item, this);

            ViewData["ContentPageId"] = new SelectList(await _service.GetAllAsync<ContentPage>("ContentPage"), "Id", "DisplayName", this.ContentPageId);
            return Page();
        }

        public async Task<IActionResult>
            OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ContentPageId"] = new SelectList(await _service.GetAllAsync<ContentPage>("ContentPage"), "Id", "DisplayName", this.ContentPageId);
                return Page();
            }

            var item = await _service.FindAsync<NavigationEntry>("NavigationEntry", this.Id);
            _mapper.Map(this, item);

            if (!(await _service.Update<NavigationEntry>("NavigationEntry", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }

    }
}
