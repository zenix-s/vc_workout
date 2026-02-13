using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Sets;

public class UpdateSet(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record UpdateSetRequest(
        int SetNumber,
        decimal Weight,
        int Reps,
        int Rpe
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/sets/{id}", HandleAsync)
            .WithName("Update set");
    }

    public async Task<IResult> HandleAsync(int id, UpdateSetRequest request)
    {
        var set = await _context.WorkoutSets.FindAsync(id);

        if (set == null)
        {
            return Results.NotFound(new { message = "Set not found" });
        }

        set.SetNumber = request.SetNumber;
        set.Weight = request.Weight;
        set.Reps = request.Reps;
        set.Rpe = request.Rpe;

        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}
