using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Exercises;

public class DeleteExercise(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/exercises/{id}", HandleAsync)
            .WithName("Delete exercise");
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var exercise = await _context.Exercises.FindAsync(id);

        if (exercise == null)
        {
            return Results.NotFound(new { message = "Exercise not found" });
        }

        // Check if exercise has sets
        var hasSets = await _context.WorkoutSets.AnyAsync(s => s.ExerciseId == id);
        if (hasSets)
        {
            return Results.BadRequest(new { message = "Cannot delete exercise with existing sets" });
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}
