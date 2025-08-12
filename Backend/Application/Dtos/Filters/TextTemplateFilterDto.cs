namespace Application.Dtos.Filters;

public class TextTemplateFilterDto
{
    public int ThemeId { get; set; }
    public CategoryType CategoryType { get; set; }
    public List<int> CategoryId { get; set; } = new List<int>();
}
