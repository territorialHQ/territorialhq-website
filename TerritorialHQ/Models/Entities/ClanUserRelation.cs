using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TerritorialHQ.Models
{
    public class ClanUserRelation : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string ClanId { get; set; }
        public virtual Clan Clan { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
