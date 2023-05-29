using System.ComponentModel.DataAnnotations.Schema;

namespace TerritorialHQ.Models
{
    public class ContentPage : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string? DisplayName { get; set; }
        public string? Content { get; set; }
        public string? SidebarContent { get; set; }

        public string? BannerImage { get; set; }

        public virtual List<NavigationEntry>? NavigationEntries { get; set; }
    }
}
