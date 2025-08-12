namespace Domain.Entities;

public class TextTemplate : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public int ThemeId { get; set; } = 0;
    public CategoryType CategoryType { get; set; } = 0;
    public int CategoryId { get; set; } = 0;

    public Theme Theme { get; set; } = default!;
}
