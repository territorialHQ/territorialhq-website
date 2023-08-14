using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly ContentPageService _service;
        private readonly IWebHostEnvironment _env;

        public CreateModel(
            IMapper mapper,
            ContentPageService service,
            IWebHostEnvironment env
        )
        {
            _mapper = mapper;
            _service = service;
            _env = env;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Display Name (internal)")]
        public string? DisplayName { get; set; }
        [BindProperty]
        [Display(Name = "HTML Content")]
        new public string? Content { get; set; }
        [BindProperty]
        [Display(Name = "HTML Sidebar Content")]
        public string? SidebarContent { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileBanner)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new DTOContentPage();
            _mapper.Map(this, item);

            if (fileBanner != null)
            {
                item.BannerImage = ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerImage, false);
            }

            if (await _service.Add<DTOContentPage>("ContentPage", item) == null)
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
