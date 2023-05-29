using System.ComponentModel.DataAnnotations.Schema;

namespace TerritorialHQ.Models
{
    public interface IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        string Id { get; set; }
    }
}
