using Microsoft.JSInterop;
using vc_workout.wasm.Models;

namespace vc_workout.wasm.Services;

public class WorkoutStateService
{
    private readonly WorkoutService _workoutService;
    private readonly IJSRuntime _jsRuntime;
    private int? _activeWorkoutId;
    private const string STORAGE_KEY = "activeWorkoutId";

    public event Action? OnChange;

    public WorkoutStateService(WorkoutService workoutService, IJSRuntime jsRuntime)
    {
        _workoutService = workoutService;
        _jsRuntime = jsRuntime;
    }

    public int? ActiveWorkoutId => _activeWorkoutId;
    public bool HasActiveWorkout => _activeWorkoutId.HasValue;

    public async Task InitializeAsync()
    {
        // Load from localStorage
        var storedId = await GetFromLocalStorageAsync();
        if (storedId.HasValue)
        {
            // Verify the workout is still active (EndDate is null)
            var workout = await _workoutService.GetByIdAsync(storedId.Value);
            if (workout != null && workout.EndDate == null)
            {
                _activeWorkoutId = storedId.Value;
            }
            else
            {
                // Workout is finished or doesn't exist, clear storage
                await ClearLocalStorageAsync();
            }
        }
        NotifyStateChanged();
    }

    public async Task StartWorkoutAsync()
    {
        var workout = await _workoutService.CreateAsync();
        if (workout != null)
        {
            _activeWorkoutId = workout.Id;
            await SaveToLocalStorageAsync(workout.Id);
            NotifyStateChanged();
        }
    }

    public async Task FinishWorkoutAsync()
    {
        if (_activeWorkoutId.HasValue)
        {
            await _workoutService.FinishAsync(_activeWorkoutId.Value);
            _activeWorkoutId = null;
            await ClearLocalStorageAsync();
            NotifyStateChanged();
        }
    }

    public async Task<WorkoutDetail?> GetActiveWorkoutDetailsAsync()
    {
        if (_activeWorkoutId.HasValue)
        {
            return await _workoutService.GetByIdAsync(_activeWorkoutId.Value);
        }
        return null;
    }

    private async Task<int?> GetFromLocalStorageAsync()
    {
        try
        {
            var value = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", STORAGE_KEY);
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var id))
            {
                return id;
            }
        }
        catch
        {
            // LocalStorage not available or error occurred
        }
        return null;
    }

    private async Task SaveToLocalStorageAsync(int workoutId)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", STORAGE_KEY, workoutId.ToString());
        }
        catch
        {
            // LocalStorage not available or error occurred
        }
    }

    private async Task ClearLocalStorageAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", STORAGE_KEY);
        }
        catch
        {
            // LocalStorage not available or error occurred
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
