using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Models;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles ="Administrator, Journalist")]
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly ApisService _service;
        private readonly IWebHostEnvironment _env;

        public CreateModel(IMapper mapper, ApisService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _service = service;
            _env = env;
        }


        [BindProperty]
        [Required]
        [Display(Name = "Title / Headline")]
        public string? Title { get; set; }

        [BindProperty]
        [Display(Name = "Subtitle / Subheadline")]
        public string? Subtitle { get; set; }

        [BindProperty]
        [Display(Name = "Publish from")]
        [DataType(DataType.Text)]
        public DateTime PublishFrom { get; set; }

        [BindProperty]
        [Display(Name = "Publish until")]
        [DataType(DataType.Text)]
        public DateTime? PublishTo { get; set; }

        [BindProperty]
        [Display(Name = "Teaser text")]
        public string? Teaser { get; set; }

        [BindProperty]
        [Display(Name = "Content")]
        public string? Body { get; set; }

        [BindProperty]
        [Display(Name = "Image")]
        public string? Image { get; set; }

        [BindProperty]
        [Display(Name = "Tags (Comma separated)")]
        public string? Tags { get; set; }

        [BindProperty]
        [Display(Name = "Sticky (keep on top)")]
        public bool IsSticky { get; set; }

        public IActionResult OnGet()
        {
            PublishFrom = DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = new JournalArticle();
            _mapper.Map(this, item);

            if (imageFile != null)
            {
                item.Image = ImageHelper.ProcessImage(imageFile, _env.WebRootPath + "/Data/Uploads/System/", true, null, true);
            }

            if (!(await _service.Add<JournalArticle>("JournalArticle", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }
    }
}
