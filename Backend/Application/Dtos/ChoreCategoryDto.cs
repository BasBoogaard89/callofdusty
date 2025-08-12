namespace Application.Dtos;

public class ChoreCategoryDto : BaseDto
{
    [Required]
    public string Description { get; set; } = string.Empty;
}
