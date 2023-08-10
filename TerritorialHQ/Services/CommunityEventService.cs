using Newtonsoft.Json;
using System.Data;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services;

public class CommunityEventService : ApisDtoService
{
    public CommunityEventService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {



    }

    public async Task<List<DTOCommunityEvent>?> GetSchedule(int days)
    {
        AddTokenHeader();

        List<DTOCommunityEvent>? result = new();
        HttpResponseMessage response = await _httpClient.GetAsync("CommunityEvent/Schedule/" + days);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = await response.Content.ReadFromJsonAsync<List<DTOCommunityEvent>>();
        }

        return result;
    }

}
