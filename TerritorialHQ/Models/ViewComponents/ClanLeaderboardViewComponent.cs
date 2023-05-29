using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.X500;
using TerritorialHQ.Services;
using TerritorialHQ.ViewModels;

namespace TerritorialHQ.Models.ViewComponents
{
    public class ClanLeaderboardViewComponent : ViewComponent
    {
        private readonly IMemoryCache _memoryCache;

        public ClanLeaderboardViewComponent(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new List<LeaderboardEntry>();
            if (_memoryCache.TryGetValue("clanleaderboard", out List<LeaderboardEntry> cachedLaderboard))
            {
                model = cachedLaderboard;
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = await client.GetAsync("https://territorial.io/clans");
                    if (request.IsSuccessStatusCode)
                    {
                        var content = await request.Content.ReadAsStringAsync();
                        var lines = content.Split(System.Environment.NewLine);

                        for (int i = 4; i <= 24; i++)
                        {
                            var fields = lines[i].Split(',');
                            if (fields.Length != 3)
                                continue;

                            model.Add(new LeaderboardEntry { Rank = int.Parse(fields[0]), Name = fields[1], Points = decimal.Parse(fields[2]) });
                        }

                        _memoryCache.Set<List<LeaderboardEntry>>("clanleaderboard", model, new TimeSpan(0, 0, 5, 0));
                    }
                }

            }

            return View(model);
        }
    }
}
