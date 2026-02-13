using vc_workout.infraestructure.Data;
using vc_workout.shared.Entities;

namespace vc_workout.api.Features.Sets;

public class CreateSet(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record CreateSetRequest(
        int WorkoutId,
        int ExerciseId,
        int SetNumber,
        decimal Weight,
        int Reps,
        int Rpe
    );

    public record CreateSetResponse(
        int Id,
        int WorkoutId,
        int ExerciseId,
        int SetNumber,
        decimal Weight,
        int Reps,
        int Rpe,
        DateTime CreatedAt
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/sets", HandleAsync)
            .WithName("Create set");
    }

    public async Task<IResult> HandleAsync(CreateSetRequest request)
    {
        // Validate workout exists
        var workout = await _context.Workouts.FindAsync(request.WorkoutId);
        if (workout == null)
        {
            return Results.BadRequest(new { message = "Workout not found" });
        }

        // Validate exercise exists
        var exercise = await _context.Exercises.FindAsync(request.ExerciseId);
        if (exercise == null)
        {
            return Results.BadRequest(new { message = "Exercise not found" });
        }

        var set = new WorkoutSet
        {
            WorkoutId = request.WorkoutId,
            ExerciseId = request.ExerciseId,
            SetNumber = request.SetNumber,
            Weight = request.Weight,
            Reps = request.Reps,
            Rpe = request.Rpe,
            CreatedAt = DateTime.UtcNow
        };

        _context.WorkoutSets.Add(set);
        await _context.SaveChangesAsync();

        var response = new CreateSetResponse(
            set.Id,
            set.WorkoutId,
            set.ExerciseId,
            set.SetNumber,
            set.Weight,
            set.Reps,
            set.Rpe,
            set.CreatedAt
        );

        return Results.Created($"/api/sets/{set.Id}", response);
    }
}
