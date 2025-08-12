namespace Infrastructure.Services;

public class RoomCategoryService(IServiceProvider sp, IMapper mapper) : BaseService<RoomCategoryDto>(sp, mapper), IRoomCategoryService
{
    public override async Task<RoomCategoryDto> Save(RoomCategoryDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new RoomCategory();

        entity.Description = dto.Description;

        var data = await repository.Save(entity);

        return mapper.Map<RoomCategoryDto>(data);
    }
}
