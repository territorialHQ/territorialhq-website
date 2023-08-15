using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.X500;
using TerritorialHQ.Services;
using TerritorialHQ.Models.ViewModels;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Models.ViewComponents
{
    public class PlayerLeaderboardViewComponent : ViewComponent
    {
        private readonly IMemoryCache _memoryCache;

        public PlayerLeaderboardViewComponent(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new List<LeaderboardEntry>();
            if (_memoryCache.TryGetValue("playerleaderboard", out List<LeaderboardEntry>? cachedLaderboard))
            {
                model = cachedLaderboard;
            }
            else
            {
                using HttpClient client = new();

                var request = await client.GetAsync("https://territorial.io/players");
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var lines = content.Split(System.Environment.NewLine);

                    for (int i = 4; i <= 13; i++)
                    {
                        var fields = lines[i].Split(',');
                        if (fields.Length != 4)
                            continue;

                        var entry = new LeaderboardEntry
                        {
                            Rank = int.Parse(fields[0]),
                            Tag = fields[1].Trim(),
                            Points = decimal.Parse(fields[2].Trim())
                        };

                        model.Add(entry);
                    }

                    _memoryCache.Set<List<LeaderboardEntry>>("playerleaderboard", model, new TimeSpan(0, 0, 5, 0));
                }

            }

            return View(model);
        }
    }
}
