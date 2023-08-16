using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services.Base;
using TerritorialHQ_Library.DTO;
using TerritorialHQ.Models.ViewModels;


namespace TerritorialHQ.Services;

public class ClanService : ApisDtoService
{
    public ClanService(IHttpContextAccessor contextAccessor, IConfiguration configuration, IMemoryCache memoryCache) : base(contextAccessor, configuration, memoryCache)
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

    public async Task<List<LeaderboardEntry>> GetRankedLeaderboard()
    {
        var leaderboardEntries = new List<LeaderboardEntry>();
        if (_memoryCache.TryGetValue("clanleaderboard", out List<LeaderboardEntry>? cachedLaderboard))
        {
            leaderboardEntries = cachedLaderboard;
        }
        else
        {
            using HttpClient client = new();

            var request = await client.GetAsync("https://territorial.io/clans");
            if (request.IsSuccessStatusCode)
            {
                List<DTOClanListEntry> clanListings = await GetClanListings("Clan/Listing") ?? new();

                var content = await request.Content.ReadAsStringAsync();
                var lines = content.Split(System.Environment.NewLine);
                int? upperBound = lines?.Length >= 250 + 4 ? 250 + 4 : lines?.Length;

                for (int i = 4; i <= upperBound - 1; i++)
                {
                    var fields = lines[i].Split(',');
                    if (fields.Length != 3)
                        continue;

                    var entry = new LeaderboardEntry
                    {
                        Rank = int.Parse(fields[0]),
                        Tag = fields[1].Trim(),
                        Points = decimal.Parse(fields[2].Trim())
                    };

                    if (!string.IsNullOrEmpty(fields[1]))
                    {
                        var clan = clanListings.FirstOrDefault(c => c.Tag == fields[1].Trim());
                        entry.LogoFile = clan?.LogoFile;
                        entry.Id = clan?.Id;
                        entry.IsPublished = clan?.IsPublished ?? false;
                    }
                    leaderboardEntries.Add(entry);
                }

                _memoryCache.Set<List<LeaderboardEntry>>("clanleaderboard", leaderboardEntries, new TimeSpan(0, 0, 5, 0));
            }

        }

        return leaderboardEntries ?? new List<LeaderboardEntry>();
    }

    public async Task<LeaderboardEntry?> GetLeaderboardEntry(string clanId)
    {
        var data = await GetRankedLeaderboard();

        return data.FirstOrDefault(x =>
        {
            return x.Id == clanId;
        });
    }
    public async Task<List<LeaderboardEntry>> GetTopClans(int number)
    {
        List<LeaderboardEntry> entries = await GetRankedLeaderboard();
        return entries.Take(number).ToList();
    }

    public async Task<decimal> GetPointsAtRank(int rank)
    {
        List<LeaderboardEntry> entries = await GetRankedLeaderboard();
        return entries.FirstOrDefault(c => c.Rank == rank)?.Points ?? 0;
    }
}