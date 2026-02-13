using vc_workout.infraestructure.Data;
using vc_workout.shared.Entities;

namespace vc_workout.api.Features.Workouts;

public class CreateWorkout(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record CreateWorkoutRequest(
        DateTime Date
    );

    public record CreateWorkoutResponse(
        int Id,
        DateTime Date
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/workouts", HandleAsync)
            .WithName("Create workout");
    }

    public async Task<IResult> HandleAsync(CreateWorkoutRequest request)
    {
        var workout = new Workout
        {
            Date = request.Date,
            EndDate = null
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        var response = new CreateWorkoutResponse(
            workout.Id,
            workout.Date
        );

        return Results.Created($"/api/workouts/{workout.Id}", response);
    }
}
