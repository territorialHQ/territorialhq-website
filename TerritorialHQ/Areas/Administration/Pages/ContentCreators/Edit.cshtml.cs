using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.ContentCreators
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly ContentCreatorService _service;
        private readonly IWebHostEnvironment _env;

        public EditModel(IMapper mapper, ContentCreatorService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _service = service;
            _env = env;
        }

        [BindProperty]
        [Required]
        public string? Id { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [BindProperty]
        [Display(Name = "Logo File")]
        public string? LogoFile { get; set; }
        [BindProperty]
        [Display(Name = "Banner File")]
        public string? BannerFile { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Channel Link")]
        public string? ChannelLink { get; set; }

        [BindProperty]
        public bool RemoveLogo { get; set; }
        [BindProperty]
        public bool RemoveBanner { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync<DTOContentCreator>("ContentCreator", id);
            if (item == null)
            {
                return NotFound();
            }

            _mapper.Map(item, this);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileLogo, IFormFile? fileBanner)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = await _service.FindAsync<DTOContentCreator>("ContentCreator", this.Id!);
            if (item == null)
                return NotFound();

            _mapper.Map(this, item);

            if (fileLogo != null)
            {
                item.LogoFile = ImageHelper.ProcessImage(fileLogo, _env.WebRootPath + "/Data/Uploads/System/", true, item.LogoFile, false);
            }
            else if (RemoveLogo && !string.IsNullOrEmpty(item.LogoFile))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.LogoFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.LogoFile = null;
                RemoveLogo = false;
            }

            if (fileBanner != null)
            {
                item.BannerFile = ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerFile, false);
            }
            else if (RemoveBanner && !string.IsNullOrEmpty(item.BannerFile))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.BannerFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.BannerFile = null;
                RemoveBanner = false;
            }

            if (!(await _service.Update<DTOContentCreator>("ContentCreator", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }

    }
}
