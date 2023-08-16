using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Services;
using TerritorialHQ_Library.DTO;

namespace TerritorialHQ.Pages.Clans
{
    public class IndexModel : PageModel
    {
        private readonly ClanService _clanService;

        public IndexModel(ClanService clanService)
        {
            _clanService = clanService;
        }

        public List<DTOClanListEntry> ClanListings { get; set; } = new();

        public async Task<IActionResult> OnGet(string sort = "rank")
        {
            ClanListings = await _clanService.GetClanListings("Clan/Listing") ?? new();

            if (sort == "rank")
            {
                using HttpClient client = new();
                var request = await client.GetAsync("https://territorial.io/clans");
                if (request.IsSuccessStatusCode)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    var lines = content.Split(System.Environment.NewLine);

                    for (int i = 4; i <= 204; i++)
                    {
                        var fields = lines[i].Split(',');
                        if (fields.Length != 3)
                            continue;

                        var clan = ClanListings.FirstOrDefault(c => c.Tag == fields[1].Trim());
                        if (clan != null)
                            clan.Position = i - 3;
                    }
                }

                foreach (var clan in ClanListings.Where(c => c.Position == null))
                    clan.Position = int.MaxValue;

                ClanListings = ClanListings.OrderBy(o => o.Position).ThenBy(o => o.Name).ToList();
            }
            else
            {
                ClanListings = ClanListings.OrderBy(o => o.Name).ToList();
            }

            return Page();
        }
    }
}
