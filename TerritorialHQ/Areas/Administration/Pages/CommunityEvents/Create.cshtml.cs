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

namespace TerritorialHQ.Areas.Administration.Pages.CommunityEvents
{
    [Authorize(Roles ="Administrator, Staff")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly CommunityEventService _service;
        private readonly IWebHostEnvironment _env;

        public CreateModel(IMapper mapper, CommunityEventService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _service = service;
            _env = env;
        }

        [BindProperty]
        [Display(Name = "Title")]
        public string? Title { get; set; }
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

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new DTOCommunityEvent();

            _mapper.Map(this, item);

            if (imageFile != null)
            {
                item.ImageFile = ImageHelper.ProcessImage(imageFile, _env.WebRootPath + "/Data/Uploads/System/", true, null, true);
            }

            var id = await _service.Add<DTOCommunityEvent>("CommunityEvent", item);
            if (id == null)
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Details", new { id });
        }
    }
}
