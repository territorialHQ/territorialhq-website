using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.DTO.Interface;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services;

public class NavigationEntryService : ApisDtoService
{
    public NavigationEntryService(IHttpContextAccessor contextAccessor, IConfiguration configuration, IMemoryCache memoryCache) : base(contextAccessor, configuration, memoryCache)
    {
    }

    public override async Task<List<DTONavigationEntry>?> GetAllAsync<DTONavigationEntry>(string endpoint) 
    {
        var navigationEntries = new List<DTONavigationEntry>();
        if (_memoryCache.TryGetValue("dtonavigationentry", out List<DTONavigationEntry>? cachedNavigationEntries))
        {

            navigationEntries = cachedNavigationEntries;
        }
        else
        {
            navigationEntries = await base.GetAllAsync<DTONavigationEntry>(endpoint);
            if (navigationEntries != null)
            {
                _memoryCache.Set<List<DTONavigationEntry>>("dtonavigationentry", navigationEntries, new TimeSpan(0, 0, 20, 0));
            }
        }

        return navigationEntries ?? new List<DTONavigationEntry>();
    }
}
