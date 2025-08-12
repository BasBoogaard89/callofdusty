namespace Domain.Entities;

public class Chore : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; } = 0;
    public int RoomId { get; set; } = 0;
    public int DurationMinutes { get; set; } = 0;
    public int FrequencyDays { get; set; } = 0;
    public DirtinessFactor DirtinessFactor { get; set; } = 0;

    public Room Room { get; set; } = null!;
    public ChoreCategory Category { get; set; } = null!;
}
