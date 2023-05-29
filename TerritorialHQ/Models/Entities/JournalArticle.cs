using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TerritorialHQ.Models
{
    public class JournalArticle : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "Title / Headline")]
        public string Title { get; set; }

        [Display(Name = "Subtitle / Subheadline")]
        public string? Subtitle { get; set; }

        [Display(Name = "Publish from")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime PublishFrom { get; set; }

        [Display(Name = "Publish until")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? PublishTo { get; set; }

        public bool IsPublished => (DateTime.Now >= PublishFrom) && (PublishTo == null ? true : DateTime.Now <= PublishTo);

        [Display(Name = "Teaser text")]
        public string? Teaser { get; set; }

        [Display(Name = "Content")]
        public string? Body { get; set; }

        [Display(Name = "Image")]
        public string? Image { get; set; }

        [Display(Name = "Tags (Comma separated)")]
        public string? Tags { get; set; }

        [Display(Name = "Sticky (keep on top)")]
        public bool IsSticky { get; set; }
    }
}
