namespace Application.Interfaces.Services;

public interface ITextFragmentService : IBaseService<TextFragmentDto>
{
    Task<List<TextFragmentDto>> GetAllFiltered(TextFragmentFilterDto filters);
}
