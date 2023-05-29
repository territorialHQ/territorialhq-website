using System.ComponentModel.DataAnnotations.Schema;

namespace TerritorialHQ.Models
{
    public class NavigationEntry : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public bool Public { get; set; }

        public string? ParentId { get; set; }
        public virtual NavigationEntry? Parent { get; set; }
        public short Order { get; set; }

        public string? ContentPageId { get; set; }
        public virtual ContentPage? ContentPage { get; set; }

        public string? ExternalUrl { get; set; }


        [InverseProperty("Parent")]
        public virtual List<NavigationEntry>? SubEntries { get; set; }
    }
}
