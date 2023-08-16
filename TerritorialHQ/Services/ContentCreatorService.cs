using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class ContentCreatorService : ApisDtoService
{
    public ContentCreatorService(IHttpContextAccessor contextAccessor, IConfiguration configuration, IMemoryCache memoryCache) : base(contextAccessor, configuration, memoryCache)
    {
    }
}
