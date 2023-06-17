using Newtonsoft.Json;
using System.Data;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.Entities;

namespace TerritorialHQ.Services
{
    public class ClanUserRelationService : ApisService
    {
        public ClanUserRelationService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
        {
        }

        public async Task<List<ClanUserRelation>?> GetByClanAsync(string clanId)
        {
            AddTokenHeader();

            List<ClanUserRelation>? result = new();
            HttpResponseMessage response = await _httpClient.GetAsync("ClanUserRelation/Clan/" + clanId);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<List<ClanUserRelation>>(jsonString);
            }

            return result;
        }
    }
}
