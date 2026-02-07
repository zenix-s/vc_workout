using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Models;

public class Set
{
    [Key]
    public int Id { get; set; }

    public int ExerciseId { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public double Weight { get; set; }

    public int Reps { get; set; }

    [ForeignKey("ExerciseId")]
    public Exercise? Exercise { get; set; }
}
