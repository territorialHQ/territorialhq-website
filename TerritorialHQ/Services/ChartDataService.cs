using System.Net.Http.Headers;

namespace TerritorialHQ.Services;

public class ChartDataService
{
    protected readonly HttpClient _httpClient;

    public ChartDataService()
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

    public async Task<List<float>> GetClanChartData(string? clanName)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("http://140.238.195.229:5000/api/" + clanName + "/pointhistory"),
        };

        var response = await _httpClient.SendAsync(request);

        List<float> result = new();
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = await response.Content.ReadFromJsonAsync<List<float>>();
        }

        return result;
    }

}
