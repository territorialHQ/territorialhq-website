using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Services;

public class ClanService : ApisDtoService
{
    public ClanService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {
    }

    public async Task<List<DTOClanListEntry>?> GetClanListings(string endpoint)
    {
        AddTokenHeader();

        List<DTOClanListEntry>? result = new();
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = await response.Content.ReadFromJsonAsync<List<DTOClanListEntry>>();
        }

        return result;
    }
}
