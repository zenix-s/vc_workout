using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Exercises;

public class GetExercises(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record ExerciseResponse(
        int Id,
        string Name,
        DateTime CreatedAt
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/exercises", HandleAsync)
            .WithName("Get all exercises");
    }

    public async Task<IResult> HandleAsync()
    {
        var exercises = await _context.Exercises
            .OrderBy(e => e.Name)
            .ToListAsync();

        var response = exercises.Select(e => new ExerciseResponse(
            e.Id,
            e.Name,
            e.CreatedAt
        ));

        return Results.Ok(response);
    }
}
