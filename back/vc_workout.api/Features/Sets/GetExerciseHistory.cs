using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Sets;

public class GetExerciseHistory(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record SetHistoryResponse(
        int WorkoutId,
        DateTime WorkoutDate,
        int SetNumber,
        decimal Weight,
        int Reps,
        int Rpe
    );

    public record WorkoutHistoryResponse(
        int WorkoutId,
        DateTime WorkoutDate,
        List<SetHistoryResponse> Sets
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/exercises/{exerciseId}/history", HandleAsync)
            .WithName("Get exercise history");
    }

    public async Task<IResult> HandleAsync(int exerciseId, int limit = 3)
    {
        // Get last N workouts where this exercise was used
        var workoutIds = await _context.WorkoutSets
            .Where(s => s.ExerciseId == exerciseId)
            .Select(s => s.WorkoutId)
            .Distinct()
            .OrderByDescending(id => id)
            .Take(limit)
            .ToListAsync();

        var history = new List<WorkoutHistoryResponse>();

        foreach (var workoutId in workoutIds)
        {
            var workout = await _context.Workouts.FindAsync(workoutId);
            if (workout == null) continue;

            var sets = await _context.WorkoutSets
                .Where(s => s.WorkoutId == workoutId && s.ExerciseId == exerciseId)
                .OrderBy(s => s.SetNumber)
                .Select(s => new SetHistoryResponse(
                    s.WorkoutId,
                    workout.Date,
                    s.SetNumber,
                    s.Weight,
                    s.Reps,
                    s.Rpe
                ))
                .ToListAsync();

            history.Add(new WorkoutHistoryResponse(
                workoutId,
                workout.Date,
                sets
            ));
        }

        return Results.Ok(history);
    }
}
