namespace Infrastructure.Services;

public class HistoryService(IServiceProvider sp, IMapper mapper) : BaseService<HistoryDto>(sp, mapper), IHistoryService
{
    public override async Task<HistoryDto> Save(HistoryDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new History();

        entity.ChoreId = dto.ChoreId;
        entity.DateStarted = dto.DateStarted;
        entity.DateCompleted = dto.DateCompleted;

        var data = await repository.Save(entity);

        return mapper.Map<HistoryDto>(data);
    }
}
