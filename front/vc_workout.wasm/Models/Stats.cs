namespace vc_workout.wasm.Models;

public class Stats
{
    public int TotalWorkouts { get; set; }
    public DateTime? LastWorkoutDate { get; set; }
    public int SetsThisMonth { get; set; }
    public int CurrentStreak { get; set; }
}
