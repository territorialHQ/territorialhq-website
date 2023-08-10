using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.CommunityEvents
{
    [Authorize(Roles = "Administrator, Staff")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly CommunityEventService _service;
        private readonly IWebHostEnvironment _env;

        public EditModel(IMapper mapper, CommunityEventService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _service = service;
            _env = env;
        }


        [BindProperty]
        [Required]
        public string? Id { get; set; }
        [BindProperty]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [BindProperty]
        [Display(Name = "Published")]
        public bool IsPublished { get; set; }
        [BindProperty]
        [Display(Name = "In Review")]
        public bool InReview { get; set; }

        [BindProperty]
        [MaxLength(250)]
        [Display(Name = "Event Description (max. 250 chars)")]
        public string? Description { get; set; }

        [BindProperty]
        [Display(Name = "Start (UTC)")]
        [DataType(DataType.Text)]
        public DateTime? Start { get; set; }
        [BindProperty]
        [Display(Name = "End (UTC)")]
        [DataType(DataType.Text)]
        public DateTime? End { get; set; }
        [BindProperty]
        [Display(Name = "Recurring Event")]
        public bool Recurring { get; set; }
        [BindProperty]
        [Display(Name = "Interval (every X days)")]
        public int? Interval { get; set; }
        [BindProperty]
        [Display(Name = "Event location (server / channel)")]
        public string? Location { get; set; }
        [BindProperty]
        [Display(Name = "Discord server invite link")]
        public string? DiscordServerLink { get; set; }
        [BindProperty]
        [Display(Name = "Host")]
        public string? Host { get; set; }

        [BindProperty]
        public string? ImageFile { get; set; }

        [BindProperty]
        public bool RemoveImage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync<DTOCommunityEvent>("CommunityEvent", id);
            if (item == null)
            {
                return NotFound();
            }

            _mapper.Map(item, this);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = await _service.FindAsync<DTOCommunityEvent>("CommunityEvent", this.Id!);
            if (item == null)
                return NotFound();

            _mapper.Map(this, item);

            if (imageFile != null)
            {
                item.ImageFile = ImageHelper.ProcessImage(imageFile, _env.WebRootPath + "/Data/Uploads/System/", true, item.ImageFile, true);
            }
            else if (RemoveImage && !string.IsNullOrEmpty(item.ImageFile))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.ImageFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.ImageFile = null;
                RemoveImage = false;
            }

            if (!User.IsInRole("Administrator"))
            {
                item.IsPublished = false;
                item.InReview = false;
            }

            if (!(await _service.Update<DTOCommunityEvent>("CommunityEvent", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Details", new { id = item.Id });
        }

    }
}
