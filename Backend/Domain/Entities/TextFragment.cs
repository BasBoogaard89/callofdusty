namespace Domain.Entities;

public class TextFragment : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int TextTemplateId { get; set; } = 0;

    public TextTemplate TextTemplate { get; set; } = default!;
}
