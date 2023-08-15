using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.DTO.Interface;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services;

public class NavigationEntryService : ApisDtoService
{
    private readonly IMemoryCache _memoryCache;
    public NavigationEntryService(IHttpContextAccessor contextAccessor, IConfiguration configuration, MemoryCache memoryCache) : base(contextAccessor, configuration)
    {
        _memoryCache = memoryCache;
    }

    public override async Task<List<DTONavigationEntry>?> GetAllAsync<DTONavigationEntry>(string endpoint) 
    {
        var model = new List<DTONavigationEntry>();
        if (_memoryCache.TryGetValue("dtonavigationentry", out List<DTONavigationEntry>? cachedNavigationEntries))
        {

            model = cachedNavigationEntries;
        }
        else
        {
            model = await base.GetAllAsync<DTONavigationEntry>(endpoint) ?? new List<DTONavigationEntry>();
            if (model != null)
            {
                _memoryCache.Set<List<DTONavigationEntry>>("dtonavigationentry", model, new TimeSpan(0, 1, 0, 0));
            }
        }

        return model ?? new List<DTONavigationEntry>();
    }
}
