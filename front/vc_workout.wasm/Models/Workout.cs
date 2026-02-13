namespace vc_workout.wasm.Models;

public class Workout
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime? EndDate { get; set; }
}

public class WorkoutListItem
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime? EndDate { get; set; }
    public int SetCount { get; set; }
}

public class WorkoutDetail
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime? EndDate { get; set; }
    public List<WorkoutSetDetail> Sets { get; set; } = new();
}

public class WorkoutSetDetail
{
    public int Id { get; set; }
    public int SetNumber { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public int ExerciseId { get; set; }
    public decimal Weight { get; set; }
    public int Reps { get; set; }
    public int Rpe { get; set; }
    public DateTime CreatedAt { get; set; }
}
