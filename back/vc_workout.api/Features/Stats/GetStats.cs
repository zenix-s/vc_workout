using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Stats;

public class GetStats(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record StatsResponse(
        int TotalWorkouts,
        DateTime? LastWorkoutDate,
        int SetsThisMonth,
        int CurrentStreak
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/stats", HandleAsync)
            .WithName("Get dashboard stats");
    }

    public async Task<IResult> HandleAsync()
    {
        // Total workouts (finished ones)
        var totalWorkouts = await _context.Workouts
            .CountAsync(w => w.EndDate != null);

        // Last workout date
        var lastWorkout = await _context.Workouts
            .Where(w => w.EndDate != null)
            .OrderByDescending(w => w.Date)
            .FirstOrDefaultAsync();

        // Sets this month
        var now = DateTime.UtcNow;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var setsThisMonth = await _context.WorkoutSets
            .CountAsync(s => s.CreatedAt >= firstDayOfMonth);

        // Current streak (consecutive days with workouts)
        var currentStreak = await CalculateStreak();

        var stats = new StatsResponse(
            totalWorkouts,
            lastWorkout?.Date,
            setsThisMonth,
            currentStreak
        );

        return Results.Ok(stats);
    }

    private async Task<int> CalculateStreak()
    {
        var finishedWorkouts = await _context.Workouts
            .Where(w => w.EndDate != null)
            .OrderByDescending(w => w.Date)
            .Select(w => w.Date.Date)
            .Distinct()
            .ToListAsync();

        if (!finishedWorkouts.Any())
            return 0;

        var streak = 1;
        var currentDate = finishedWorkouts[0];

        for (int i = 1; i < finishedWorkouts.Count; i++)
        {
            var previousDate = finishedWorkouts[i];
            var daysDiff = (currentDate - previousDate).Days;

            if (daysDiff == 1)
            {
                streak++;
                currentDate = previousDate;
            }
            else
            {
                break;
            }
        }

        return streak;
    }
}
