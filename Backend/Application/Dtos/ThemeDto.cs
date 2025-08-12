namespace Application.Dtos;

public class ThemeDto : BaseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
