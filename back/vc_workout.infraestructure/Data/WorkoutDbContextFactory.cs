using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace vc_workout.infraestructure.Data;

public class WorkoutDbContextFactory : IDesignTimeDbContextFactory<WorkoutDbContext>
{
    public WorkoutDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkoutDbContext>();
        optionsBuilder.UseSqlite("Data Source=workout.db");

        return new WorkoutDbContext(optionsBuilder.Options);
    }
}
