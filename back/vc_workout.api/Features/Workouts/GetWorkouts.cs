using Microsoft.EntityFrameworkCore;
using vc_workout.infraestructure.Data;

namespace vc_workout.api.Features.Workouts;

public class GetWorkouts(WorkoutDbContext context) : ISlice
{
    private readonly WorkoutDbContext _context = context;

    public record WorkoutResponse(
        int Id,
        DateTime Date,
        DateTime? EndDate,
        int SetCount
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/workouts", HandleAsync)
            .WithName("Get workouts");
    }

    public async Task<IResult> HandleAsync(int page = 1, int pageSize = 10)
    {
        var skip = (page - 1) * pageSize;

        var workouts = await _context.Workouts
            .OrderByDescending(w => w.Date)
            .Skip(skip)
            .Take(pageSize)
            .Select(w => new WorkoutResponse(
                w.Id,
                w.Date,
                w.EndDate,
                _context.WorkoutSets.Count(s => s.WorkoutId == w.Id)
            ))
            .ToListAsync();

        var totalCount = await _context.Workouts.CountAsync();

        return Results.Ok(new
        {
            data = workouts,
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }
}
