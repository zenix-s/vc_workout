using System.Net.Http.Json;
using vc_workout.wasm.Models;

namespace vc_workout.wasm.Services;

public class ExerciseService
{
    private readonly HttpClient _http;

    public ExerciseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Exercise>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Exercise>>("/api/exercises") ?? new List<Exercise>();
    }

    public async Task<Exercise?> CreateAsync(string name)
    {
        var response = await _http.PostAsJsonAsync("/api/exercises", new { name });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Exercise>();
    }

    public async Task<Exercise?> UpdateAsync(int id, string name)
    {
        var response = await _http.PutAsJsonAsync($"/api/exercises/{id}", new { name });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Exercise>();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"/api/exercises/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ExerciseHistoryWorkout>> GetHistoryAsync(int exerciseId, int limit = 3)
    {
        return await _http.GetFromJsonAsync<List<ExerciseHistoryWorkout>>($"/api/exercises/{exerciseId}/history?limit={limit}") 
            ?? new List<ExerciseHistoryWorkout>();
    }
}
