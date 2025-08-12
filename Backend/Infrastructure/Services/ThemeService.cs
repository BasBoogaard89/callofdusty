namespace Infrastructure.Services;

public class ThemeService(IServiceProvider sp, IMapper mapper) : BaseService<ThemeDto>(sp, mapper), IThemeService
{
    public override async Task<ThemeDto> Save(ThemeDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new Theme();

        entity.Description = dto.Description;

        var data = await repository.Save(entity);

        return mapper.Map<ThemeDto>(data);
    }
}
