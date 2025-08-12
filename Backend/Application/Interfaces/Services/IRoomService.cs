namespace Application.Interfaces.Services;

public interface IRoomService : IBaseService<RoomDto>
{
    Task<List<RoomDto>> GetAllFiltered(RoomFilterDto filters);
}
