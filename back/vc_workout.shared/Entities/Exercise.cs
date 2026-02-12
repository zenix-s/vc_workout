namespace vc_workout.shared.Entities;

public class Exercise
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
