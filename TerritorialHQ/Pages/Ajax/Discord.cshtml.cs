using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using TerritorialHQ.Models.Cache;
using TerritorialHQ.Services;

namespace TerritorialHQ.Pages.Ajax
{
    public class DiscordModel : PageModel
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DiscordBotService _discordBotService;

        public DiscordModel(IMemoryCache memoryCache, DiscordBotService discordBotService)
        {
            _memoryCache = memoryCache;
            _discordBotService = discordBotService;
        }

        public async Task<IActionResult> OnGetDiscordUserData(ulong id)
        {
            var rnd = new Random();

            DiscordCacheModel model = new DiscordCacheModel();

            if (_memoryCache.TryGetValue(id, out DiscordCacheModel discordCache))
            {
                model = discordCache;
            }
            else
            {

#if !DEBUG
                await Task.Delay(rnd.Next(1000, 5000));

                var userData = await _discordBotService.GetDiscordUserAsync(id);
                if (userData != null)
                {
                    model.Username = userData.Username + "#" + userData.Discriminator;
                    model.Avatar = userData.AvatarHash;
                    _memoryCache.Set(id, new DiscordCacheModel() { Username = model.Username, Avatar = userData.AvatarHash }, new TimeSpan(0, rnd.Next(12, 24), 0, 0));

                }
#endif

            }

            return new JsonResult(model);
        }

    }
}

