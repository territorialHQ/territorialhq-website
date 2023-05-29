using Microsoft.EntityFrameworkCore;
using TerritorialHQ.Data;
using TerritorialHQ.Models;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services
{
    public class JournalArticleService : BaseService<JournalArticle>
    {
        public JournalArticleService(ApplicationDbContext context, LoggerService logger) : base(context, logger)
        {

        }

        public override async Task<IList<JournalArticle>> GetAllAsync()
        {
            return await Query.OrderByDescending(o => o.PublishFrom).ThenBy(o => o.PublishTo).ToListAsync();
        }
    }
}
