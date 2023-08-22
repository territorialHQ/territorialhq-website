using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly ClanService _service;
        private readonly IWebHostEnvironment _env;

        public CreateModel(IMapper mapper, ClanService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _service = service;
            _env = env;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [BindProperty]
        [Display(Name = "Discord Guild ID")]
        public string? GuildId { get; set; }
        [BindProperty]
        [Display(Name = "Date / Period of Foundation")]
        public string? Foundation { get; set; }
        [BindProperty]
        [Display(Name = "Founder(s)")]
        public string? Founders { get; set; }
        [BindProperty]
        [Display(Name = "Clan Motto")]
        public string? Motto { get; set; }
        [BindProperty]
        [Display(Name = "Primary Color")]
        public string? Color1 { get; set; }
        [BindProperty]
        [Display(Name = "Secondary Color")]
        public string? Color2 { get; set; }
        [BindProperty]
        [Display(Name = "Use colors for clan page")]
        public bool UseColorForPage { get; set; }
        [BindProperty]
        [Display(Name = "Clan Tag (without [ ])")]
        public string? Tag { get; set; }
        [BindProperty]
        [Display(Name = "Custom Bot HttpGet Endpoint")]
        public string? BotEndpoint { get; set; }
        [BindProperty]
        [Display(Name = "Logo file (1 MB max)")]
        public string? LogoFile { get; set; }
        [BindProperty]
        [Display(Name = "Banner file / video (5 MB max)")]
        public string? BannerFile { get; set; }
        [BindProperty]
        [Display(Name = "Discord server link")]
        public string? DiscordLink { get; set; }
        [BindProperty]
        [Display(Name = "Overview (500 characters max)")]
        [MaxLength(500)]
        public string? Overview { get; set; }
        [BindProperty]
        [Display(Name = "General Description")]
        public string? Description { get; set; }
        [BindProperty]
        [Display(Name = "Clan History")]
        public string? History { get; set; }
        [BindProperty]
        [Display(Name = "Community")]
        public string? Community { get; set; }
        [BindProperty]
        [Display(Name = "Features")]
        public string? Features { get; set; }
        [BindProperty]
        [Display(Name = "Miscellaneous")]
        public string? Miscellaneous { get; set; }


        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileLogo, IFormFile? fileBanner)
        {
            if (fileLogo != null && fileLogo.Length > 1048576)
                ModelState.AddModelError("LogoFile", "Logo file too large (max. 1MB)");
            if (fileBanner != null && fileBanner.Length > 5242880)
                ModelState.AddModelError("BannerFile", "Banner file too large (max. 5MB)");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new DTOClan();
            _mapper.Map(this, item);

            if (fileLogo != null)
            {
                item.LogoFile = ImageHelper.ProcessImage(fileLogo, _env.WebRootPath + "/Data/Uploads/System/", true, item.LogoFile, false);
            }

            if (fileBanner != null)
            {
                if (fileBanner.FileName.EndsWith(".mp4"))
                {
                    var filename = Guid.NewGuid().ToString() + ".mp4";
                    string filePath = _env.WebRootPath + "/Data/Uploads/System/" + filename;
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileBanner.CopyToAsync(fileStream);
                    }
                    item.BannerFile = filename;
                }
                else
                {
                    item.BannerFile = ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerFile, false);
                }
            }

            if (await _service.Add<DTOClan>("Clan", item) == null)
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
