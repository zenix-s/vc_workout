using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Workouts;

public class FinishWorkout(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/workouts/{id}/finish", HandleAsync)
            .WithName("Finish workout");
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var workout = await _context.Workouts.FindAsync(id);

        if (workout == null)
        {
            return Results.NotFound(new { message = "Workout not found" });
        }

        workout.EndDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}
