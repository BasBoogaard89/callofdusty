namespace Infrastructure.Services;

public class TextFragmentService(IServiceProvider sp, IMapper mapper) : BaseService<TextFragmentDto>(sp, mapper), ITextFragmentService
{
    public override async Task<TextFragmentDto> Save(TextFragmentDto dto)
    {
        var entity = await repository.GetById(dto.Id) ?? new TextFragment();

        entity.Key = dto.Key;
        entity.Value = dto.Value;
        entity.TextTemplateId = dto.TextTemplateId;

        var data = await repository.Save(entity);

        return mapper.Map<TextFragmentDto>(data);
    }

    public async Task<List<TextFragmentDto>> GetAllFiltered(TextFragmentFilterDto filters)
    {
        var data = await repository.GetAllFiltered(filters);

        return mapper.Map<List<TextFragmentDto>>(data);
    }
}
