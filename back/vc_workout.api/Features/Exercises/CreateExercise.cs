using vc_workout.api.Features;

namespace vc_workout.api.Features.Exercises;

public class CreateExercise : ISlice
{
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

    public Task<IResult> HandleAsync(CreateExerciseRequest request)
    {
        throw new NotImplementedException();
    }
}
