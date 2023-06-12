using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly ApisService _service;
        private readonly IWebHostEnvironment _env;

        public EditModel(IMapper mapper, ApisService service, IWebHostEnvironment env)
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

        [BindProperty]
        [Display(Name = "Published")]
        public bool IsPublished { get; set; }
        [BindProperty]
        [Display(Name = "In Review")]
        public bool InReview { get; set; }

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

            var item = await _service.FindAsync<Clan>("Clan", id);
            if (item == null)
            {
                return NotFound();
            }

            var userRelations = await _service.GetAllAsync<ClanUserRelation>("ClanUserRelation") ?? new List<ClanUserRelation>();

            if (!User.IsInRole("Administrator") && !userRelations.Any(r => r.AppUser.UserName == User.Identity?.Name))
                return Forbid();

            if (User.IsInRole("Administrator") && userRelations.Count > 0)
                return Forbid();
            
            _mapper.Map(item, this);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileLogo, IFormFile? fileBanner)
        {
            var item = await _service.FindAsync<Clan>("Clan", this.Id!);
            if (item == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Page();
            }

            if (!User.IsInRole("Administrator"))
            {
                this.Name = item.Name;
                this.GuildId = item.GuildId;
            }

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

            if (!User.IsInRole("Administrator"))
            {
                item.IsPublished = false;
                item.InReview = false;
            }

            if (!(await _service.Update<Clan>("Clan", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Details", new { id = item.Id });
        }

    }
}
