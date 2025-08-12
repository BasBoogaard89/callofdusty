namespace Infrastructure.Repositories;

public class TextTemplateRepository(AppDbContext context) : BaseRepository<TextTemplate>(context), ITextTemplateRepository
{
    public async Task<TextTemplate> GetRandomTextTemplate(int themeId, CategoryType categoryType, int? categoryId = null)
    {
        return await context.TextTemplate
            .Where(e => e.ThemeId == themeId && e.CategoryType == categoryType)
            .WhereIf(categoryId != null, (e) => e.CategoryId == categoryId)
            .OrderBy(e => Guid.NewGuid())
            .FirstOrDefaultAsync();
    }

    public async Task<List<TextTemplate>> GetAllFiltered(TextTemplateFilterDto filters)
    {
        var filteredData = await context.TextTemplate
            .Where(e => e.CategoryType == filters.CategoryType)
            .WhereIf(filters.ThemeId != 0, e => e.ThemeId == filters.ThemeId)
            .WhereIf(filters.CategoryId.Count() > 0, e => filters.CategoryId.Contains(e.CategoryId))
            .ToListAsync();

        return filteredData;
    }
}
