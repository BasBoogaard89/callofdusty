namespace Application.Interfaces.Repositories;

public interface IRoomRepository : IBaseRepository<Room>
{
    Task<List<Room>> GetAllFiltered(RoomFilterDto filters);
}
