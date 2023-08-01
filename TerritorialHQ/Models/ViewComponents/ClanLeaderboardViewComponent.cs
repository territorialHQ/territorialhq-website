using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.X500;
using TerritorialHQ.Services;
using TerritorialHQ.ViewModels;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Models.ViewComponents
{
    public class ClanLeaderboardViewComponent : ViewComponent
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ClanService _clanService;
        public ClanLeaderboardViewComponent(IMemoryCache memoryCache, ClanService clanService)
        {
            _memoryCache = memoryCache;
            _clanService = clanService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new List<LeaderboardEntry>();
            if (_memoryCache.TryGetValue("clanleaderboard", out List<LeaderboardEntry>? cachedLaderboard))
            {
                model = cachedLaderboard;
            }
            else
            {
                using HttpClient client = new();

                var request = await client.GetAsync("https://territorial.io/clans");
                if (request.IsSuccessStatusCode)
                {
                    List<DTOClanListEntry> clanListings = await _clanService.GetClanListings("Clan/Listing") ?? new();

                    var content = await request.Content.ReadAsStringAsync();
                    var lines = content.Split(System.Environment.NewLine);

                    for (int i = 4; i <= 13; i++)
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
                        model.Add(entry);
                    }

                    _memoryCache.Set<List<LeaderboardEntry>>("clanleaderboard", model, new TimeSpan(0, 0, 5, 0));
                }

            }

            return View(model);
        }
    }
}
