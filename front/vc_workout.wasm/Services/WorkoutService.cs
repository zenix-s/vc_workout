using System.Net.Http.Json;
using vc_workout.wasm.Models;

namespace vc_workout.wasm.Services;

public class WorkoutService
{
    private readonly HttpClient _http;

    public WorkoutService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PagedResult<WorkoutListItem>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        return await _http.GetFromJsonAsync<PagedResult<WorkoutListItem>>($"/api/workouts?page={page}&pageSize={pageSize}") 
            ?? new PagedResult<WorkoutListItem>();
    }

    public async Task<WorkoutDetail?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<WorkoutDetail>($"/api/workouts/{id}");
    }

    public async Task<Workout?> CreateAsync(DateTime? date = null)
    {
        var workoutDate = date ?? DateTime.Now;
        var response = await _http.PostAsJsonAsync("/api/workouts", new { date = workoutDate });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Workout>();
    }

    public async Task<Workout?> FinishAsync(int id)
    {
        var response = await _http.PutAsync($"/api/workouts/{id}/finish", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Workout>();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"/api/workouts/{id}");
        response.EnsureSuccessStatusCode();
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
