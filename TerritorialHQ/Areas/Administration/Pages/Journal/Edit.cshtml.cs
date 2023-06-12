using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles = "Administrator, Journalist")]
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


        [BindProperty]
        public bool RemoveImage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync<JournalArticle>("JournalArticle", id);
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
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return Page();
            }

            var item = await _service.FindAsync<JournalArticle>("JournalArticle", this.Id);
            if (item == null)
                return NotFound();

            _mapper.Map(this, item);

            if (imageFile != null)
            {
                item.Image = ImageHelper.ProcessImage(imageFile, _env.WebRootPath + "/Data/Uploads/System/", true, item.Image, true);
            }
            else if (RemoveImage && !string.IsNullOrEmpty(item.Image))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.Image);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.Image = null;
                RemoveImage = false;
            }

            if (!(await _service.Update<JournalArticle>("JournalArticle", item)))
                throw new Exception("Error while saving data set.");

            return RedirectToPage("./Index");
        }

    }
}
