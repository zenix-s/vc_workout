using System.Net.Http.Json;
using vc_workout.wasm.Models;

namespace vc_workout.wasm.Services;

public class StatsService
{
    private readonly HttpClient _http;

    public StatsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Stats?> GetStatsAsync()
    {
        return await _http.GetFromJsonAsync<Stats>("/api/stats");
    }
}
