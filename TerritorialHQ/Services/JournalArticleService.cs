using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class JournalArticleService : ApisDtoService
{
    public JournalArticleService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {
    }
}
