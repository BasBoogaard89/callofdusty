namespace Infrastructure.Repositories;

public class TextFragmentRepository(AppDbContext context) : BaseRepository<TextFragment>(context), ITextFragmentRepository
{
    public async Task<List<TextFragment>> GetAllFiltered(TextFragmentFilterDto filters)
    {
        var filteredData = await context.TextFragment
            .WhereIf(filters.TextTemplateIds.Count() > 0, e => filters.TextTemplateIds.Contains(e.TextTemplateId))
            .ToListAsync();

        return filteredData;
    }
}
