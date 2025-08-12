namespace Application.Dtos;

public class TextTemplateDto : BaseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int ThemeId { get; set; } = 0;

    [Required]
    public CategoryType CategoryType { get; set; } = 0;

    [Required]
    public int CategoryId { get; set; } = 0;
}
