namespace Application.Dtos;

public class RoomDto : BaseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    public List<ChoreDto> Chores { get; set; } = [];

    public RoomCategoryDto? Category { get; set; }
}
