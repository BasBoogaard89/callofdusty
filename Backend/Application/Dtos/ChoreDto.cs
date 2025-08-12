namespace Application.Dtos;

public class ChoreDto : BaseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int RoomId { get; set; }

    [Required]
    public int DurationMinutes { get; set; }

    [Required]
    public int FrequencyDays { get; set; }

    [Required]
    public DirtinessFactor DirtinessFactor { get; set; }

    public RoomDto? Room { get; set; } = null!;

    public ChoreCategoryDto? Category { get; set; } = null!;
}
