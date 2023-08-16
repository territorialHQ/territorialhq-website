using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class ContentPageService : ApisDtoService
{
    public ContentPageService(IHttpContextAccessor contextAccessor, IConfiguration configuration, IMemoryCache memoryCache) : base(contextAccessor, configuration, memoryCache)
    {
    }
}
