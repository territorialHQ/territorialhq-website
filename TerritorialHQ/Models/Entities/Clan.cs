using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TerritorialHQ.Models
{
    public class Clan : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Discord Guild ID")]
        public ulong? GuildId { get; set; }
        [Display(Name = "Logo file")]
        public string? LogoFile { get; set; }
        [Display(Name = "Banner file")]
        public string? BannerFile { get; set; }
        [Display(Name = "Discord server link")]
        public string? DiscordLink { get; set; }
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Published")]
        public bool IsPublished { get; set; }
        [Display(Name = "In Review")]
        public bool InReview { get; set; }

        public virtual List<ClanUserRelation> ClanUserRelations { get; set; }
    }
}
