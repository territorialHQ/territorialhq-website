using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class JournalArticleService : ApisDtoService
{
    public JournalArticleService(IHttpContextAccessor contextAccessor, IConfiguration configuration, IMemoryCache memoryCache) : base(contextAccessor, configuration, memoryCache)
    {
    }
}
