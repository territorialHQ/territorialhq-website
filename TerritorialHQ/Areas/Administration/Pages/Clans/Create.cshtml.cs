using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ClanService _service;
        private readonly IWebHostEnvironment _env;

        public CreateModel(IMapper mapper, LoggerService logger, ClanService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _env = env;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [BindProperty]
        [Display(Name = "Discord Guild ID")]
        public ulong? GuildId { get; set; }
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


        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileLogo, IFormFile? fileBanner)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new Clan();
            _mapper.Map(this, item);

            if (fileLogo != null)
            {
                item.LogoFile = await ImageHelper.ProcessImage(fileLogo, _env.WebRootPath + "/Data/Uploads/System/", true, item.LogoFile, false);
            }

            if (fileBanner != null)
            {
                item.BannerFile = await ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerFile, false);
            }

            _service.Add(item);
            await _service.SaveChangesAsync(User);

            return RedirectToPage("./Details", new { id = item.Id });
        }
    }
}
