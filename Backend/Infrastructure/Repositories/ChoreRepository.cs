namespace Infrastructure.Repositories;

public class ChoreRepository(AppDbContext context) : BaseRepository<Chore>(context), IChoreRepository
{
    public async Task<List<Chore>> GetAllFiltered(ChoreFilterDto filters)
    {
        var filteredData = await context.Chore
            .WhereIf(filters.RoomId != 0, e => e.RoomId == filters.RoomId)
            .ToListAsync();

        return filteredData;
    }
}
