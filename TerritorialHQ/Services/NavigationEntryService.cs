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

    public override async Task<string?> Add<T>(string endpoint, T item)
    {
        _memoryCache.Remove("dtonavigationentry");
        return await base.Add(endpoint, item);
    }

    public override async Task<bool> Update<T>(string endpoint, T item)
    {
        _memoryCache.Remove("dtonavigationentry");
        return await base.Update(endpoint, item);
    }

    public override async Task<bool> Remove(string? endpoint, string? id)
    {
        _memoryCache.Remove("dtonavigationentry");
        return await base.Remove(endpoint, id);
    }
}
