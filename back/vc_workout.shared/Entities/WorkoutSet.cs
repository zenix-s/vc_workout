namespace vc_workout.shared.Entities;

public class WorkoutSet
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int ExerciseId { get; set; }
    public int SetNumber { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public int Rpe { get; set; }
    public DateTime CreatedAt { get; set; }
}
