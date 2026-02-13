using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Sets;

public class DeleteSet(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/sets/{id}", HandleAsync)
            .WithName("Delete set");
    }

    public async Task<IResult> HandleAsync(int id)
    {
        var set = await _context.WorkoutSets.FindAsync(id);

        if (set == null)
        {
            return Results.NotFound(new { message = "Set not found" });
        }

        _context.WorkoutSets.Remove(set);
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}
