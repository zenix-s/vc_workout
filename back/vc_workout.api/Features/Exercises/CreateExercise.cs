using vc_workout.api.Features;
using vc_workout.infraestructure.Data;
using vc_workout.shared.Entities;

namespace vc_workout.api.Features.Exercises;

public class CreateExercise(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record CreateExerciseRequest(
        string Name
    );

    public record CreateExerciseResponse(
        Guid Id,
        string Name
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/exercises", HandleAsync)
            .WithName("Create exercise");
    }

    public async Task<IResult> HandleAsync(CreateExerciseRequest request)
    {
        var exercise = new Exercise
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();

        var response = new CreateExerciseResponse(
            exercise.Id,
            exercise.Name
        );

        return Results.Created($"/api/exercises/{exercise.Id}", response);
    }
}

