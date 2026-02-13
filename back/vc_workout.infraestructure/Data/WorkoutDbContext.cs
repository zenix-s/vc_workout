using Microsoft.EntityFrameworkCore;
using vc_workout.shared.Entities;

namespace vc_workout.infraestructure.Data;

public class WorkoutDbContext : DbContext
{
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutSet> WorkoutSets => Set<WorkoutSet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Date).IsRequired();
            entity.Property(w => w.EndDate).IsRequired(false);
        });

        modelBuilder.Entity<WorkoutSet>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.WorkoutId).IsRequired();
            entity.Property(s => s.ExerciseId).IsRequired();
            entity.Property(s => s.SetNumber).IsRequired();
            entity.Property(s => s.Weight).IsRequired().HasPrecision(18, 2);
            entity.Property(s => s.Reps).IsRequired();
            entity.Property(s => s.Rpe).IsRequired();
            entity.Property(s => s.CreatedAt).IsRequired();

            entity.HasOne<Workout>()
                .WithMany()
                .HasForeignKey(s => s.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Exercise>()
                .WithMany()
                .HasForeignKey(s => s.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
