namespace Infrastructure.Repositories;

public class HistoryRepository(AppDbContext context) : BaseRepository<History>(context), IHistoryRepository
{
    public async Task<Dictionary<int, DateTime>> GetLastCompletedPerChore()
    {
        return await context.History
            .GroupBy(h => h.ChoreId)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.Max(x => x.DateCompleted)
            );
    }
}
