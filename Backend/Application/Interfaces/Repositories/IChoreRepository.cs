namespace Application.Interfaces.Repositories;

public interface IChoreRepository : IBaseRepository<Chore>
{
    Task<List<Chore>> GetAllFiltered(ChoreFilterDto filters);
}
