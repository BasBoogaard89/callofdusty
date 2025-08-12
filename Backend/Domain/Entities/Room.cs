namespace Domain.Entities;

public class Room : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; } = 0;

    public List<Chore> Chores { get; set; } = [];
    public RoomCategory Category { get; set; } = null!;
}
