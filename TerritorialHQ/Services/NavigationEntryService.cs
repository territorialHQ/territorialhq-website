using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using TerritorialHQ.Data;
using TerritorialHQ.Models;
using TerritorialHQ.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace TerritorialHQ.Services
{
    public class NavigationEntryService : BaseService<NavigationEntry>
    {
        public NavigationEntryService(ApplicationDbContext context, LoggerService logger) : base(context, logger)
        {

        }

        public async Task<List<NavigationEntry>> GetTopLevelAsync()
        {
            return await Query.Where(q => q.ParentId == null).OrderBy(o => o.Order).ToListAsync();
        }

        public async Task<NavigationEntry> GetBySlugAsync(string slug)
        {
            return await Query.FirstOrDefaultAsync(q => q.Slug == slug);
        }
    }
}
