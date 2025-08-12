namespace Application.Interfaces.Repositories;

public interface ITextTemplateRepository : IBaseRepository<TextTemplate>
{
    Task<TextTemplate> GetRandomTextTemplate(int themeId, CategoryType categoryType, int? categoryId = null);
    Task<List<TextTemplate>> GetAllFiltered(TextTemplateFilterDto filters);
}
