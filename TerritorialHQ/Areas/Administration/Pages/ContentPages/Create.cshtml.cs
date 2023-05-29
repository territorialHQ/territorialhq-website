using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ContentPageService _service;
        private readonly IWebHostEnvironment _env;

        public CreateModel(
            IMapper mapper, LoggerService logger,
            ContentPageService service,
            IWebHostEnvironment env
        )
        {
            _mapper = mapper;
            _logger = logger;
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

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileBanner)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new ContentPage();
            _mapper.Map(this, item);

            if (fileBanner != null)
            {
                item.BannerImage = await ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerImage, false);
            }

            _service.Add(item);
            await _service.SaveChangesAsync(User);

            return RedirectToPage("./Index");
        }
    }
}
