namespace Application.Dtos;

public class TextFragmentDto : BaseDto
{
    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Value { get; set; } = string.Empty;

    [Required]
    public int TextTemplateId { get; set; } = 0;
}
