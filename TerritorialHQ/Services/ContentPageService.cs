using TerritorialHQ.Data;
using TerritorialHQ.Models;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services
{
    public class ContentPageService : BaseService<ContentPage>
    {
        public ContentPageService(ApplicationDbContext context, LoggerService logger) : base(context, logger)
        {

        }
    }
}
