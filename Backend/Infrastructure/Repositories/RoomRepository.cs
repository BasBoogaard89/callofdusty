namespace Infrastructure.Repositories;

public class RoomRepository(AppDbContext context) : BaseRepository<Room>(context), IRoomRepository
{
    public async Task<List<Room>> GetAllFiltered(RoomFilterDto filters)
    {
        var filteredData = await context.Room
            //.WhereIf(filters.OnlyWithActiveVacancies, c => c.Chores.Count > 0)
            .ToListAsync();

        return filteredData;
    }
}
