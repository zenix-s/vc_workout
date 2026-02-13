using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Exercises;

public class UpdateExercise(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record UpdateExerciseRequest(
        string Name
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/exercises/{id}", HandleAsync)
            .WithName("Update exercise");
    }

    public async Task<IResult> HandleAsync(int id, UpdateExerciseRequest request)
    {
        var exercise = await _context.Exercises.FindAsync(id);

        if (exercise == null)
        {
            return Results.NotFound(new { message = "Exercise not found" });
        }

        exercise.Name = request.Name;
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}
