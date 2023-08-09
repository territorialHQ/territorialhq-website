using System.Net.Http.Headers;
using System.Text.Json;
using TerritorialHQ.Models.ViewModels;

namespace TerritorialHQ.Services;

public class BotEndpointService
{
    protected readonly HttpClient _httpClient;

    public BotEndpointService()
    {
#if (DEBUG)
        // Deactivate any SSL certificate validation in development because it never fucking works 

        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler);
#else
        _httpClient = new HttpClient();
#endif
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<uint> GetPlayerPoints(string? endpoint, string? playerId)
    {
        if (endpoint == null || playerId == null)
            return 0;

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(endpoint + (endpoint.EndsWith("/") ? "" : "/") + playerId),
        };

        var response = await _httpClient.SendAsync(request);

        uint result = 0;
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            _ = uint.TryParse(content, out result);
        }

        return result;
    }

    public async Task<DiscordServerInfo> GetServerStats(string? invite)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://discord.com/api/invites/" + invite + "?with_counts=true"),
        };

        var response = await _httpClient.SendAsync(request);

        DiscordServerInfo result = null;
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            result = JsonSerializer.Deserialize<DiscordServerInfo>(content);
        }

        return result;
    }
}
