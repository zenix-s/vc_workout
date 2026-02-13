using System.Net.Http.Json;
using vc_workout.wasm.Models;

namespace vc_workout.wasm.Services;

public class SetService
{
    private readonly HttpClient _http;

    public SetService(HttpClient http)
    {
        _http = http;
    }

    public async Task<WorkoutSet?> CreateAsync(int workoutId, int exerciseId, int setNumber, decimal weight, int reps, int rpe)
    {
        var response = await _http.PostAsJsonAsync("/api/sets", new 
        { 
            workoutId, 
            exerciseId, 
            setNumber, 
            weight, 
            reps, 
            rpe 
        });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutSet>();
    }

    public async Task<WorkoutSet?> UpdateAsync(int id, decimal weight, int reps, int rpe)
    {
        var response = await _http.PutAsJsonAsync($"/api/sets/{id}", new { weight, reps, rpe });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WorkoutSet>();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"/api/sets/{id}");
        response.EnsureSuccessStatusCode();
    }
}
