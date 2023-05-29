using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Journal
{
    [Authorize(Roles ="Administrator, Journalist")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly JournalArticleService _service;
        private readonly IWebHostEnvironment _env;

        public EditModel(IMapper mapper, LoggerService logger, JournalArticleService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _env = env;
        }


        [BindProperty]
        public string Id { get; set; }

        [BindProperty]
        [Display(Name = "Title / Headline")]
        public string Title { get; set; }

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

            var item = await _service.FindAsync(id);
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

            var item = await _service.FindAsync(this.Id);
            _mapper.Map(this, item);

            if (imageFile != null)
            {
                item.Image = await ImageHelper.ProcessImage(imageFile, _env.WebRootPath + "/Data/Uploads/System/", true, item.Image, true);
            }
            else if (RemoveImage && !string.IsNullOrEmpty(item.Image))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.Image);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.Image = null;
                RemoveImage = false;
            }

            _service.Update(item);

            try
            {
                await _service.SaveChangesAsync(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await JournalArticleExists(Id);
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
            JournalArticleExists(string id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
