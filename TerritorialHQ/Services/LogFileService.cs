using Microsoft.AspNetCore.Mvc;
using TerritorialHQ.Services.Base;

namespace TerritorialHQ.Services;

public class LogFileService : ApisService
{
    public LogFileService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(contextAccessor, configuration)
    {

    }

    public async Task<List<string>> GetFilesAsync()
    {
        AddTokenHeader();

        List<string> result = new();
        HttpResponseMessage response = await _httpClient.GetAsync("LogFile/List");

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
        }

        return result;
    }

    public async Task<byte[]?> GetFile(string name)
    {
        AddTokenHeader();

        byte[]? result = null;
        HttpResponseMessage response = await _httpClient.GetAsync("LogFile/" + name);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
             result = await response.Content.ReadAsByteArrayAsync();
        }

        return result;
    }

    public async Task<byte[]?> GetSqlBackup()
    {
        AddTokenHeader();

        byte[]? result = null;
        HttpResponseMessage response = await _httpClient.GetAsync("LogFile/SqlBackup");

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            result = await response.Content.ReadAsByteArrayAsync();
        }

        return result;
    }

    public override Task<T?> FindAsync<T>(string endpoint, string id) where T : default
    {
        return base.FindAsync<T>(endpoint, id);
    }
}
