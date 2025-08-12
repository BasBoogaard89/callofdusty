namespace Infrastructure.Services;

public class ChoreCategoryService(IServiceProvider sp, IMapper mapper) : BaseService<ChoreCategoryDto>(sp, mapper), IChoreCategoryService
{
    public override async Task<ChoreCategoryDto> Save(ChoreCategoryDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new ChoreCategory();

        entity.Description = dto.Description;

        var data = await repository.Save(entity);

        return mapper.Map<ChoreCategoryDto>(data);
    }
}
