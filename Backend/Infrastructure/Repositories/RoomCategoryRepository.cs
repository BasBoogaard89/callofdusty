namespace Infrastructure.Repositories;

public class RoomCategoryRepository(AppDbContext context) : BaseRepository<RoomCategory>(context), IRoomCategoryRepository
{
}
