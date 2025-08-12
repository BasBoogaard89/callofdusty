namespace Infrastructure.Services;

public class RoomService(IServiceProvider sp, IMapper mapper) : BaseService<RoomDto>(sp, mapper), IRoomService
{
    public override async Task<RoomDto> Save(RoomDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new Room();

        entity.CategoryId = dto.CategoryId;
        entity.Description = dto.Description;

        var data = await repository.Save(entity);

        return mapper.Map<RoomDto>(data);
    }

    public async Task<List<RoomDto>> GetAllFiltered(RoomFilterDto filters)
    {
        var data = await repository.GetAllFiltered(filters);

        return mapper.Map<List<RoomDto>>(data);
    }
}
