namespace Application.Interfaces.Repositories;

public interface ITextFragmentRepository : IBaseRepository<TextFragment>
{
    Task<List<TextFragment>> GetAllFiltered(TextFragmentFilterDto filters);
}
