namespace Application.Dtos;

public class RoomCategoryDto : BaseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
