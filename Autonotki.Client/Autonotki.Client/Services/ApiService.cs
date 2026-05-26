using System.Net.Http.Json;
using Autonotki.Application.DTOs;

namespace Autonotki.Client.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService()
    {
        _http = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }

    public async Task<List<ZlecenieDto>> GetZleceniaAsync()
    {
        try { return await _http.GetFromJsonAsync<List<ZlecenieDto>>("/zlecenia") ?? []; }
        catch { return []; }
    }

    public async Task<List<KalendarzDayDto>> GetKalendarzAsync(int year, int month)
    {
        try { return await _http.GetFromJsonAsync<List<KalendarzDayDto>>($"/kalendarz/{year}/{month}") ?? []; }
        catch { return []; }
    }

    public Task<HttpResponseMessage> CreateZlecenieAsync(CreateZlecenieRequest req) =>
        _http.PostAsJsonAsync("/zlecenia", req);

    public Task<HttpResponseMessage> UpdateStatusAsync(int id, string status) =>
        _http.PutAsJsonAsync($"/zlecenia/{id}/status", new StatusUpdateRequest(status));

    public Task<HttpResponseMessage> DeleteZlecenieAsync(int id) =>
        _http.DeleteAsync($"/zlecenia/{id}");
}
