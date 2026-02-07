using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public List<Set> Sets { get; set; } = new();
}
