using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Workouts;

public class GetWorkoutById(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record WorkoutSetResponse(
        int Id,
        int ExerciseId,
        string ExerciseName,
        int SetNumber,
        decimal Weight,
        int Reps,
        int Rpe,
        DateTime CreatedAt
    );

    public record WorkoutDetailResponse(
        int Id,
        DateTime Date,
        DateTime? EndDate,
        List<WorkoutSetResponse> Sets
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/workouts/{id}", HandleAsync)
            .WithName("Get workout by id");
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var workout = await _context.Workouts.FindAsync(id);

        if (workout == null)
        {
            return Results.NotFound(new { message = "Workout not found" });
        }

        var sets = await _context.WorkoutSets
            .Where(s => s.WorkoutId == id)
            .Join(_context.Exercises,
                s => s.ExerciseId,
                e => e.Id,
                (s, e) => new WorkoutSetResponse(
                    s.Id,
                    s.ExerciseId,
                    e.Name,
                    s.SetNumber,
                    s.Weight,
                    s.Reps,
                    s.Rpe,
                    s.CreatedAt
                ))
            .OrderBy(s => s.CreatedAt)
            .ToListAsync();

        var response = new WorkoutDetailResponse(
            workout.Id,
            workout.Date,
            workout.EndDate,
            sets
        );

        return Results.Ok(response);
    }
}
