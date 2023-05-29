using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.ContentPages
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ContentPageService _service;
        private readonly IWebHostEnvironment _env;

        public EditModel(IMapper mapper, LoggerService logger, ContentPageService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _env = env;
        }


        [BindProperty]
        public string Id { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Display Name (internal)")]
        public string? DisplayName { get; set; }
        [BindProperty]
        [Display(Name = "Banner file")]
        public string? BannerImage { get; set; }
        [BindProperty]
        [Display(Name = "HTML Content")]
        new public string? Content { get; set; }
        [BindProperty]
        [Display(Name = "HTML Sidebar Content")]
        public string? SidebarContent { get; set; }


        [BindProperty]
        public bool RemoveBanner { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _mapper.Map(item, this);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileBanner)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = await _service.FindAsync(this.Id);
            _mapper.Map(this, item);

            if (fileBanner != null)
            {
                item.BannerImage = await ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerImage, false);
            }
            else if (RemoveBanner && !string.IsNullOrEmpty(item.BannerImage))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.BannerImage);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.BannerImage = null;
                RemoveBanner = false;
            }

            _service.Update(item);

            try
            {
                await _service.SaveChangesAsync(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await ContentPageExists(Id);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool>
            ContentPageExists(string id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
