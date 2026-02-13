using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Workouts;

public class DeleteWorkout(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/workouts/{id}", HandleAsync)
            .WithName("Delete workout");
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var workout = await _context.Workouts.FindAsync(id);

        if (workout == null)
        {
            return Results.NotFound(new { message = "Workout not found" });
        }

        // Delete associated sets first (cascade should handle this, but explicit is better)
        var sets = await _context.WorkoutSets.Where(s => s.WorkoutId == id).ToListAsync();
        _context.WorkoutSets.RemoveRange(sets);

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}
