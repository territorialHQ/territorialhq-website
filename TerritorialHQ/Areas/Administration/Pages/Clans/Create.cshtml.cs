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
        public ulong? GuildId { get; set; }
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
        [Display(Name = "Custom Bot HttpGet Endpoint")]
        public string? BotEndpoint { get; set; }
        [BindProperty]
        [Display(Name = "Logo file")]
        public string? LogoFile { get; set; }
        [BindProperty]
        [Display(Name = "Banner file")]
        public string? BannerFile { get; set; }
        [BindProperty]
        [Display(Name = "Discord server link")]
        public string? DiscordLink { get; set; }
        [BindProperty]
        [Display(Name = "Description")]
        public string? Description { get; set; }


        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileLogo, IFormFile? fileBanner)
        {
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
                item.BannerFile = ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerFile, false);
            }

            if (!(await _service.Add<DTOClan>("Clan", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
