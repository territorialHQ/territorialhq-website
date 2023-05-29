using Microsoft.EntityFrameworkCore;
using TerritorialHQ.Data;
using TerritorialHQ.Models;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services
{
    public class ClanService : BaseService<Clan>
    {
        public ClanService(ApplicationDbContext context, LoggerService logger) : base(context, logger)
        {

        }

        public override async Task<IList<Clan>> GetAllAsync()
        {
            return await Query.OrderBy(o => o.Name).ToListAsync();
        }
    }

    public class ClanUserRelationService : BaseService<ClanUserRelation>
    {
        public ClanUserRelationService(ApplicationDbContext context, LoggerService logger) : base(context, logger)
        {

        }
    }
}
