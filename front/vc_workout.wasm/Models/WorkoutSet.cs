namespace vc_workout.wasm.Models;

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

public class ExerciseHistoryWorkout
{
    public int WorkoutId { get; set; }
    public DateTime WorkoutDate { get; set; }
    public List<ExerciseHistorySet> Sets { get; set; } = new();
}

public class ExerciseHistorySet
{
    public int SetNumber { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public int Rpe { get; set; }
}
