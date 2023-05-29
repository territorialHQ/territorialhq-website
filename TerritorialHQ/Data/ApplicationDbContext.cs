using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TerritorialHQ.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TerritorialHQ.Models.Clan> Clans { get; set; }
        public DbSet<TerritorialHQ.Models.ClanUserRelation> ClanUserRelations { get; set; }
        public DbSet<TerritorialHQ.Models.NavigationEntry> NavigationEntries { get; set; }
        public DbSet<TerritorialHQ.Models.ContentPage> ContentPages { get; set; }
        public DbSet<TerritorialHQ.Models.JournalArticle> JournalArticles { get; set; }
    }
}